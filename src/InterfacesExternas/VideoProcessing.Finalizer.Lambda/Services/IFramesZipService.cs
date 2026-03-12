namespace VideoProcessing.Finalizer.Lambda.Services;

/// <summary>
/// Contrato do pipeline de consolidação de frames: listar, baixar, compactar em ZIP e enviar ao S3.
/// </summary>
public interface IFramesZipService
{
    Task<IReadOnlyList<string>> ListAllFrameKeysAsync(string bucket, string basePrefix, CancellationToken cancellationToken = default);
    Task<string> DownloadFramesAsync(string bucket, IEnumerable<string> keys, string basePrefix, CancellationToken cancellationToken = default);
    void CreateZip(string framesDir, string zipPath, bool ordenaAutomaticamente = true);
    Task<string> UploadZipToS3Async(string outputBucket, string zipLocalPath, string zipS3Key, CancellationToken cancellationToken = default);
}
