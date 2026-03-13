using System.IO.Compression;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using FluentAssertions;
using NSubstitute;
using VideoProcessing.Finalizer.Infra.Services;
using Xunit;

namespace VideoProcessing.Finalizer.Tests;

public class FramesZipServiceTests
{
    private readonly IAmazonS3 _s3 = Substitute.For<IAmazonS3>();

    [Fact]
    public void GetJobIdFromPrefix_ComMultiplosSegmentos_RetornaUltimoSegmento()
    {
        var result = FramesZipService.GetJobIdFromPrefix("processed/uuid1/uuid2");

        result.Should().Be("uuid2");
    }

    [Fact]
    public void GetJobIdFromPrefix_ComUmSegmento_RetornaOProprio()
    {
        var result = FramesZipService.GetJobIdFromPrefix("job123");

        result.Should().Be("job123");
    }

    [Fact]
    public void BuildZipS3Key_ComOutputBasePrefixEVideoId_RetornaChaveNoFormatoEsperado()
    {
        var key = FramesZipService.BuildZipS3Key("abc123/def456", "def456");

        key.Should().Be("abc123/def456/def456_frames.zip");
    }

    [Fact]
    public void BuildZipS3Key_ComOutputBasePrefixComBarraFinal_NormalizaSemBarraDupla()
    {
        var key = FramesZipService.BuildZipS3Key("guidUsuario/guidVideoId/", "vid-1");

        key.Should().Be("guidUsuario/guidVideoId/vid-1_frames.zip");
    }

    [Fact]
    public async Task ListAllFrameKeysAsync_ComPaginacao_RetornaTodasAsChavesDeImagem()
    {
        var prefix = "processed/job1/";
        _s3.ListObjectsV2Async(Arg.Any<ListObjectsV2Request>(), Arg.Any<CancellationToken>())
            .Returns(
                new ListObjectsV2Response
                {
                    S3Objects = [new S3Object { Key = $"{prefix}chunk-001/frames/f1.jpg" }],
                    IsTruncated = true,
                    NextContinuationToken = "next"
                },
                new ListObjectsV2Response
                {
                    S3Objects = [new S3Object { Key = $"{prefix}chunk-002/frames/f2.png" }],
                    IsTruncated = false
                });

        var sut = new FramesZipService(_s3, Path.GetTempPath());
        var keys = await sut.ListAllFrameKeysAsync("bucket", "processed/job1");

        keys.Should().HaveCount(2);
        keys.Should().Contain(k => k.EndsWith("f1.jpg"));
        keys.Should().Contain(k => k.EndsWith("f2.png"));
    }

    [Fact]
    public async Task ListAllFrameKeysAsync_FiltraApenasImagens_JpegPngIncluidos()
    {
        var prefix = "p/";
        _s3.ListObjectsV2Async(Arg.Any<ListObjectsV2Request>(), Arg.Any<CancellationToken>())
            .Returns(new ListObjectsV2Response
            {
                S3Objects =
                [
                    new S3Object { Key = $"{prefix}a.jpg" },
                    new S3Object { Key = $"{prefix}b.JPEG" },
                    new S3Object { Key = $"{prefix}c.png" },
                    new S3Object { Key = $"{prefix}d.txt" }
                ],
                IsTruncated = false
            });

        var sut = new FramesZipService(_s3, Path.GetTempPath());
        var keys = await sut.ListAllFrameKeysAsync("bucket", "p");

        keys.Should().HaveCount(3);
        keys.Should().NotContain(k => k.EndsWith(".txt"));
    }

    [Fact]
    public async Task ListAllFrameKeysAsync_NenhumFrame_LancaInvalidOperationException()
    {
        _s3.ListObjectsV2Async(Arg.Any<ListObjectsV2Request>(), Arg.Any<CancellationToken>())
            .Returns(new ListObjectsV2Response { S3Objects = [], IsTruncated = false });

        var sut = new FramesZipService(_s3, Path.GetTempPath());
        var act = () => sut.ListAllFrameKeysAsync("bucket", "prefix");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Nenhum frame encontrado*");
    }

    [Fact]
    public void CreateZip_DiretorioComArquivos_GeraZipComEntradas()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var zipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".zip");
        try
        {
            Directory.CreateDirectory(tempDir);
            File.WriteAllText(Path.Combine(tempDir, "a.jpg"), "fake");
            File.WriteAllText(Path.Combine(tempDir, "b.png"), "fake");

            var sut = new FramesZipService(_s3, Path.GetTempPath());
            sut.CreateZip(tempDir, zipPath);

            File.Exists(zipPath).Should().BeTrue();
            using (var archive = ZipFile.OpenRead(zipPath))
            {
                archive.Entries.Should().HaveCount(2);
            }
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            if (File.Exists(zipPath)) File.Delete(zipPath);
        }
    }

    [Fact]
    public void CreateZip_ComOrdenaAutomaticamenteTrue_ColocaFramesNaRaizComNomesRenomeados()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var zipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".zip");
        try
        {
            var chunk1 = Path.Combine(tempDir, "chunk-001", "frames");
            var chunk2 = Path.Combine(tempDir, "chunk-002", "frames");
            Directory.CreateDirectory(chunk1);
            Directory.CreateDirectory(chunk2);
            File.WriteAllText(Path.Combine(chunk1, "frame_0001_20s.jpg"), "fake");
            File.WriteAllText(Path.Combine(chunk2, "frame_0001_0s.jpg"), "fake");
            File.WriteAllText(Path.Combine(chunk1, "frame_0002_5s.jpg"), "fake");

            var sut = new FramesZipService(_s3, Path.GetTempPath());
            sut.CreateZip(tempDir, zipPath, ordenaAutomaticamente: true);

            File.Exists(zipPath).Should().BeTrue();
            using (var archive = ZipFile.OpenRead(zipPath))
            {
                archive.Entries.Should().HaveCount(3);
                var names = archive.Entries.Select(e => e.Name).ToList();
                archive.Entries.Should().OnlyContain(e => !e.FullName.Contains('/'));
                names.Should().Contain("frame_0001_0s.jpg");
                names.Should().Contain("frame_0002_5s.jpg");
                names.Should().Contain("frame_0003_20s.jpg");
            }
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            if (File.Exists(zipPath)) File.Delete(zipPath);
        }
    }

    [Fact]
    public void CreateZip_ComOrdenaAutomaticamenteFalse_PreservaEstruturaRelativa()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var zipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".zip");
        try
        {
            var subDir = Path.Combine(tempDir, "chunk-001", "frames");
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(subDir, "frame_0001_0s.jpg"), "fake");

            var sut = new FramesZipService(_s3, Path.GetTempPath());
            sut.CreateZip(tempDir, zipPath, ordenaAutomaticamente: false);

            using (var archive = ZipFile.OpenRead(zipPath))
            {
                archive.Entries.Should().HaveCount(1);
                archive.Entries[0].FullName.Should().Contain("chunk-001");
                archive.Entries[0].FullName.Should().Contain("frames");
            }
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            if (File.Exists(zipPath)) File.Delete(zipPath);
        }
    }

    [Fact]
    public void CreateZip_DiretorioVazio_LancaInvalidOperationException()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var zipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".zip");
        try
        {
            Directory.CreateDirectory(tempDir);

            var sut = new FramesZipService(_s3, Path.GetTempPath());
            var act = () => sut.CreateZip(tempDir, zipPath);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*vazio*");
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public async Task UploadZipToS3Async_ChamaPutObjectComParametrosCorretos()
    {
        var tempZip = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".zip");
        try
        {
            File.WriteAllBytes(tempZip, [0x50, 0x4B, 0x05, 0x06]); // ZIP empty end

            var sut = new FramesZipService(_s3, Path.GetTempPath());
            await sut.UploadZipToS3Async("out-bucket", tempZip, "guidUsuario/guidVideoId/vid_frames.zip");

            await _s3.Received(1).PutObjectAsync(Arg.Is<PutObjectRequest>(r =>
                r.BucketName == "out-bucket" &&
                r.Key == "guidUsuario/guidVideoId/vid_frames.zip" &&
                r.FilePath == tempZip), Arg.Any<CancellationToken>());
        }
        finally
        {
            if (File.Exists(tempZip)) File.Delete(tempZip);
        }
    }

    [Fact]
    public async Task DownloadFramesAsync_ListaVazia_LancaInvalidOperationException()
    {
        var sut = new FramesZipService(_s3, Path.GetTempPath());
        var act = () => sut.DownloadFramesAsync("bucket", [], "prefix");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Nenhum frame encontrado*");
    }

    [Fact]
    public async Task DownloadFramesAsync_ComChaves_CriaArquivosLocaisComEstruturaRelativa()
    {
        var basePrefix = "processed/job1/";
        var key = $"{basePrefix}chunk-001/frames/f1.jpg";
        var response = new GetObjectResponse();
        var ms = new MemoryStream(Encoding.UTF8.GetBytes("fake-jpg-content"));
        response.ResponseStream = ms;

        _s3.GetObjectAsync(Arg.Is<GetObjectRequest>(r => r.BucketName == "bucket" && r.Key == key), Arg.Any<CancellationToken>())
            .Returns(response);

        var tempBase = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        try
        {
            var sut = new FramesZipService(_s3, tempBase);
            var localDir = await sut.DownloadFramesAsync("bucket", [key], basePrefix);

            localDir.Should().Contain("frames");
            var expectedFile = Path.Combine(localDir, "chunk-001", "frames", "f1.jpg");
            File.Exists(expectedFile).Should().BeTrue();
            (await File.ReadAllTextAsync(expectedFile)).Should().Be("fake-jpg-content");
        }
        finally
        {
            if (Directory.Exists(tempBase))
                Directory.Delete(tempBase, true);
        }
    }
}
