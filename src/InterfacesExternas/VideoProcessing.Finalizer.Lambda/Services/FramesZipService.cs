using System.IO.Compression;
using Amazon.S3;
using Amazon.S3.Model;

namespace VideoProcessing.Finalizer.Lambda.Services;

/// <summary>
/// Pipeline S3 → /tmp → ZIP → S3: lista frames por prefixo, baixa, compacta em ZIP e envia ao bucket de saída.
/// </summary>
public sealed class FramesZipService(IAmazonS3 s3Client, string tempBasePath = "/tmp") : IFramesZipService
{
    private const string FramesSubDir = "frames";
    private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png"];

    /// <summary>
    /// Lista todas as chaves S3 de imagem (.jpg, .jpeg, .png) sob o prefixo base, paginando até o fim.
    /// </summary>
    public async Task<IReadOnlyList<string>> ListAllFrameKeysAsync(string bucket, string basePrefix, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bucket);
        ArgumentException.ThrowIfNullOrWhiteSpace(basePrefix);

        var prefix = basePrefix.TrimEnd('/') + "/";
        var keys = new List<string>();
        string? continuationToken = null;

        do
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucket,
                Prefix = prefix,
                ContinuationToken = continuationToken
            };

            var response = await s3Client.ListObjectsV2Async(request, cancellationToken).ConfigureAwait(false);

            foreach (var obj in response.S3Objects)
            {
                if (obj.Key is null) continue;
                if (IsImageKey(obj.Key))
                    keys.Add(obj.Key);
            }

            continuationToken = response.IsTruncated ? response.NextContinuationToken : null;
        } while (continuationToken is not null);

        if (keys.Count == 0)
            throw new InvalidOperationException("Nenhum frame encontrado no prefixo informado.");

        return keys;
    }

    /// <summary>
    /// Limpa e recria o diretório de frames local; baixa cada chave preservando a estrutura relativa ao prefixo base.
    /// </summary>
    public async Task<string> DownloadFramesAsync(string bucket, IEnumerable<string> keys, string basePrefix, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bucket);
        ArgumentException.ThrowIfNullOrWhiteSpace(basePrefix);

        var keyList = keys.ToList();
        if (keyList.Count == 0)
            throw new InvalidOperationException("Nenhum frame encontrado no prefixo informado.");

        var localDir = Path.Combine(tempBasePath, FramesSubDir);
        if (Directory.Exists(localDir))
            Directory.Delete(localDir, true);
        Directory.CreateDirectory(localDir);

        var prefixTrimmed = basePrefix.TrimEnd('/') + "/";

        foreach (var key in keyList)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var relativePath = key.StartsWith(prefixTrimmed, StringComparison.Ordinal)
                ? key[prefixTrimmed.Length..]
                : Path.GetFileName(key);
            var localPath = Path.Combine(localDir, relativePath);
            var localFileDir = Path.GetDirectoryName(localPath);
            if (!string.IsNullOrEmpty(localFileDir) && !Directory.Exists(localFileDir))
                Directory.CreateDirectory(localFileDir);

            var response = await s3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = bucket,
                Key = key
            }, cancellationToken).ConfigureAwait(false);

            await using var fileStream = File.Create(localPath);
            await response.ResponseStream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
        }

        return localDir;
    }

    /// <summary>
    /// Cria um ZIP em zipPath a partir do diretório de frames. Usa CompressionLevel.Fastest e includeBaseDirectory: false.
    /// </summary>
    public void CreateZip(string framesDir, string zipPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(framesDir);
        ArgumentException.ThrowIfNullOrWhiteSpace(zipPath);

        if (!Directory.Exists(framesDir))
            throw new InvalidOperationException($"Diretório de frames não existe: {framesDir}.");

        var files = Directory.GetFiles(framesDir, "*", SearchOption.AllDirectories);
        if (files.Length == 0)
            throw new InvalidOperationException("Diretório de frames está vazio; não é possível criar o ZIP.");

        var zipDir = Path.GetDirectoryName(zipPath);
        if (!string.IsNullOrEmpty(zipDir) && !Directory.Exists(zipDir))
            Directory.CreateDirectory(zipDir);

        ZipFile.CreateFromDirectory(
            framesDir,
            zipPath,
            CompressionLevel.Fastest,
            includeBaseDirectory: false);
    }

    /// <summary>
    /// Envia o arquivo ZIP local para o bucket S3 na chave informada.
    /// </summary>
    public async Task<string> UploadZipToS3Async(string outputBucket, string zipLocalPath, string zipS3Key, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputBucket);
        ArgumentException.ThrowIfNullOrWhiteSpace(zipLocalPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(zipS3Key);

        if (!File.Exists(zipLocalPath))
            throw new FileNotFoundException("Arquivo ZIP não encontrado para upload.", zipLocalPath);

        await s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = outputBucket,
            Key = zipS3Key,
            FilePath = zipLocalPath
        }, cancellationToken).ConfigureAwait(false);

        return zipS3Key;
    }

    /// <summary>
    /// Obtém o jobId a partir do último segmento do framesBasePrefix (ex.: "processed/uuid1/uuid2" → "uuid2").
    /// </summary>
    public static string GetJobIdFromPrefix(string framesBasePrefix)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(framesBasePrefix);
        var segments = framesBasePrefix.Trim().TrimEnd('/').Split(['/', '\\'], StringSplitOptions.RemoveEmptyEntries);
        return segments.Length > 0 ? segments[^1] : framesBasePrefix.Trim();
    }

    /// <summary>
    /// Monta a chave S3 de destino do ZIP: {outputBasePrefix}/{videoId}_frames.zip
    /// </summary>
    public static string BuildZipS3Key(string outputBasePrefix, string videoId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputBasePrefix);
        ArgumentException.ThrowIfNullOrWhiteSpace(videoId);
        var prefix = outputBasePrefix.Trim().TrimEnd('/');
        return $"{prefix}/{videoId}_frames.zip";
    }

    private static bool IsImageKey(string key)
    {
        var ext = Path.GetExtension(key);
        return ext.Length > 0 && ImageExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
    }
}
