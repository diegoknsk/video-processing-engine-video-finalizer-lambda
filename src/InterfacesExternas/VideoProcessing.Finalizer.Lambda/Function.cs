using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.S3;
using VideoProcessing.Finalizer.Lambda.Models;
using VideoProcessing.Finalizer.Lambda.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace VideoProcessing.Finalizer.Lambda;

public class Function
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly IFramesZipService _framesZipService;

    public Function() : this(new AmazonS3Client())
    {
    }

    public Function(IAmazonS3 s3Client) : this(new FramesZipService(s3Client))
    {
    }

    public Function(IFramesZipService framesZipService)
    {
        _framesZipService = framesZipService;
    }

    /// <summary>
    /// Handler que consolida frames de múltiplos chunks S3 em um único ZIP e envia ao bucket de saída.
    /// Aceita payload direto (Step Functions) ou envelope SQS (Body com JSON do FinalizerInput).
    /// </summary>
    public async Task<FinalizerResult> FunctionHandler(Stream input, ILambdaContext context)
    {
        string? framesDir = null;
        string? zipPath = null;

        try
        {
            var inputModel = await ParseInputAsync(input).ConfigureAwait(false);
            ValidateInput(inputModel);

            var bucket = inputModel.FramesBucket!;
            var basePrefix = inputModel.FramesBasePrefix!;
            var outputBucket = inputModel.OutputBucket!;
            var videoId = inputModel.VideoId!;
            var outputBasePrefix = inputModel.OutputBasePrefix!;
            zipPath = Path.Combine("/tmp", $"{videoId}_frames.zip");

            context.Logger.LogInformation($"Iniciando consolidação de frames. Bucket={bucket}, Prefix={basePrefix}");

            var keys = await _framesZipService.ListAllFrameKeysAsync(bucket, basePrefix).ConfigureAwait(false);
            context.Logger.LogInformation($"Frames listados: {keys.Count}");

            framesDir = await _framesZipService.DownloadFramesAsync(bucket, keys, basePrefix).ConfigureAwait(false);
            context.Logger.LogInformation($"Frames baixados em {framesDir}");

            _framesZipService.CreateZip(framesDir, zipPath);
            context.Logger.LogInformation($"ZIP criado: {zipPath}");

            var zipS3Key = FramesZipService.BuildZipS3Key(outputBasePrefix, videoId);
            var uploadedKey = await _framesZipService.UploadZipToS3Async(outputBucket, zipPath, zipS3Key).ConfigureAwait(false);
            context.Logger.LogInformation($"ZIP enviado ao S3: {uploadedKey}");

            return new FinalizerResult(uploadedKey, keys.Count);
        }
        finally
        {
            try
            {
                if (zipPath is not null && File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                    context.Logger.LogInformation("ZIP temporário removido.");
                }
                if (framesDir is not null && Directory.Exists(framesDir))
                {
                    Directory.Delete(framesDir, true);
                    context.Logger.LogInformation("Diretório de frames removido.");
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogWarning($"Erro durante limpeza de temporários: {ex.Message}");
            }
        }
    }

    private static async Task<FinalizerInput> ParseInputAsync(Stream input)
    {
        using var doc = await JsonDocument.ParseAsync(input).ConfigureAwait(false);
        var root = doc.RootElement;

        string jsonToParse;
        if (root.TryGetProperty("Records", out var records) && records.ValueKind == JsonValueKind.Array && records.GetArrayLength() > 0)
        {
            var first = records[0];
            if (first.TryGetProperty("body", out var body))
                jsonToParse = body.GetString() ?? "{}";
            else
                jsonToParse = root.GetRawText();
        }
        else
            jsonToParse = root.GetRawText();

        var model = JsonSerializer.Deserialize<FinalizerInput>(jsonToParse, JsonOptions);
        if (model is null)
            throw new ArgumentException("Payload inválido: não foi possível deserializar FinalizerInput.");
        return model;
    }

    private static void ValidateInput(FinalizerInput input)
    {
        if (string.IsNullOrWhiteSpace(input.FramesBucket))
            throw new ArgumentException("O campo 'framesBucket' é obrigatório e não pode ser vazio.", nameof(input));
        if (string.IsNullOrWhiteSpace(input.FramesBasePrefix))
            throw new ArgumentException("O campo 'framesBasePrefix' é obrigatório e não pode ser vazio.", nameof(input));
        if (string.IsNullOrWhiteSpace(input.OutputBucket))
            throw new ArgumentException("O campo 'outputBucket' é obrigatório e não pode ser vazio.", nameof(input));
        if (string.IsNullOrWhiteSpace(input.VideoId))
            throw new ArgumentException("O campo 'videoId' é obrigatório e não pode ser vazio.", nameof(input));
        if (string.IsNullOrWhiteSpace(input.OutputBasePrefix))
            throw new ArgumentException("O campo 'outputBasePrefix' é obrigatório e não pode ser vazio.", nameof(input));
    }
}

/// <summary>
/// Resultado da execução: chave S3 do ZIP e quantidade de frames consolidados.
/// </summary>
public record FinalizerResult(string ZipS3Key, int FramesCount);
