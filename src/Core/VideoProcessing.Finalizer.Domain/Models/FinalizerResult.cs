namespace VideoProcessing.Finalizer.Domain.Models;

/// <summary>
/// Resultado da execução: bucket de saída, chave S3 do ZIP e quantidade de frames consolidados.
/// </summary>
public record FinalizerResult(string ZipBucket, string ZipS3Key, int FramesCount);
