using FluentAssertions;
using VideoProcessing.Finalizer.Lambda.Services;
using Xunit;

namespace VideoProcessing.Finalizer.Tests;

public class FrameRenamerTests
{
    [Theory]
    [InlineData("frame_0001_0s.jpg", 0)]
    [InlineData("frame_0010_120s.png", 120)]
    [InlineData("chunk-001/frames/frame_0001_20s.jpeg", 20)]
    public void TryParseTimeSeconds_ComSufixoValido_RetornaTrueESeconds(string fileName, int expectedSeconds)
    {
        FrameRenamer.TryParseTimeSeconds(fileName, out var seconds).Should().BeTrue();
        seconds.Should().Be(expectedSeconds);
    }

    [Theory]
    [InlineData("thumbnail.png")]
    [InlineData("frame_0001.jpg")]
    [InlineData("frame_0001_20.jpg")]
    [InlineData("frame_0001_20s")]
    [InlineData("")]
    public void TryParseTimeSeconds_SemSufixoValido_RetornaFalse(string fileName)
    {
        FrameRenamer.TryParseTimeSeconds(fileName, out var seconds).Should().BeFalse();
        seconds.Should().Be(-1);
    }

    [Fact]
    public void GenerateRenamedEntries_MultiplosChunks_OrdenaPorTempoERenumeracaoSequencial()
    {
        var baseDir = Path.GetTempPath();
        var paths = new[]
        {
            Path.Combine(baseDir, "chunk1", "frames", "frame_0001_20s.jpg"),
            Path.Combine(baseDir, "chunk2", "frames", "frame_0001_0s.jpg"),
            Path.Combine(baseDir, "chunk1", "frames", "frame_0002_5s.jpg")
        };

        var result = FrameRenamer.GenerateRenamedEntries(paths);

        result.Should().HaveCount(3);
        result[0].EntryName.Should().Be("frame_0001_0s.jpg");
        result[1].EntryName.Should().Be("frame_0002_5s.jpg");
        result[2].EntryName.Should().Be("frame_0003_20s.jpg");
    }

    [Fact]
    public void GenerateRenamedEntries_ArquivoSemTempo_MantemNomeOriginal()
    {
        var paths = new[]
        {
            Path.Combine("/tmp", "chunk1", "thumbnail.png"),
            Path.Combine("/tmp", "chunk1", "frame_0001_10s.jpg")
        };

        var result = FrameRenamer.GenerateRenamedEntries(paths);

        result.Should().HaveCount(2);
        result[0].EntryName.Should().Be("frame_0001_10s.jpg");
        result[1].EntryName.Should().Be("thumbnail.png");
    }

    [Fact]
    public void GenerateRenamedEntries_EmpateDeInstante_DesempateEstavelPorNomeOriginal()
    {
        var baseDir = Path.GetTempPath();
        var paths = new[]
        {
            Path.Combine(baseDir, "chunk2", "frame_0001_5s.jpg"),
            Path.Combine(baseDir, "chunk1", "frame_0001_5s.jpg")
        };

        var result = FrameRenamer.GenerateRenamedEntries(paths);

        result.Should().HaveCount(2);
        result[0].EntryName.Should().Be("frame_0001_5s.jpg");
        result[1].EntryName.Should().Be("frame_0002_5s.jpg");
        result[0].LocalPath.Should().Contain("chunk1");
        result[1].LocalPath.Should().Contain("chunk2");
    }

    [Fact]
    public void GenerateRenamedEntries_ListaVazia_RetornaListaVazia()
    {
        var result = FrameRenamer.GenerateRenamedEntries(Array.Empty<string>());
        result.Should().BeEmpty();
    }

    [Fact]
    public void GenerateRenamedEntries_MaisDe9999Frames_PaddingComCincoDigitos()
    {
        var baseDir = Path.GetTempPath();
        var paths = Enumerable.Range(0, 10001)
            .Select(i => Path.Combine(baseDir, "chunk1", "frames", $"frame_{i:D4}_{i}s.jpg"))
            .ToList();

        var result = FrameRenamer.GenerateRenamedEntries(paths);

        result.Should().HaveCount(10001);
        result[0].EntryName.Should().Be("frame_00001_0s.jpg");
        result[9999].EntryName.Should().Be("frame_10000_9999s.jpg");
        result[10000].EntryName.Should().Be("frame_10001_10000s.jpg");
    }
}
