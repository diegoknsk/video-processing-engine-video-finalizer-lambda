using System.Text.Json;
using FluentAssertions;
using VideoProcessing.Finalizer.Domain.Models;
using Xunit;

namespace VideoProcessing.Finalizer.Tests;

public class FinalizerInputTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    [Fact]
    public void Deserialize_ComPayloadValido_PreencheTodasAsPropriedades()
    {
        var json = """{"framesBucket": "bucket-x", "framesBasePrefix": "processed/abc/def", "outputBucket": "bucket-output", "videoId": "vid-123", "outputBasePrefix": "guidUsuario/guidVideoId"}""";

        var result = JsonSerializer.Deserialize<FinalizerInput>(json, JsonOptions);

        result.Should().NotBeNull();
        result!.FramesBucket.Should().Be("bucket-x");
        result.FramesBasePrefix.Should().Be("processed/abc/def");
        result.OutputBucket.Should().Be("bucket-output");
        result.VideoId.Should().Be("vid-123");
        result.OutputBasePrefix.Should().Be("guidUsuario/guidVideoId");
    }

    [Fact]
    public void Deserialize_SemVideoId_PropriedadeFicaNula()
    {
        var json = """{"framesBucket": "x", "framesBasePrefix": "y", "outputBucket": "z", "outputBasePrefix": "a/b"}""";
        var result = JsonSerializer.Deserialize<FinalizerInput>(json, JsonOptions);
        result!.VideoId.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Deserialize_SemOutputBasePrefix_PropriedadeFicaNula()
    {
        var json = """{"framesBucket": "x", "framesBasePrefix": "y", "outputBucket": "z", "videoId": "v1"}""";
        var result = JsonSerializer.Deserialize<FinalizerInput>(json, JsonOptions);
        result!.OutputBasePrefix.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Deserialize_SemFramesBucket_PropriedadeFicaNula()
    {
        var json = """{"framesBasePrefix": "x", "outputBucket": "y"}""";
        var result = JsonSerializer.Deserialize<FinalizerInput>(json, JsonOptions);
        result!.FramesBucket.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Deserialize_SemFramesBasePrefix_PropriedadeFicaNula()
    {
        var json = """{"framesBucket": "x", "outputBucket": "y"}""";
        var result = JsonSerializer.Deserialize<FinalizerInput>(json, JsonOptions);
        result!.FramesBasePrefix.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Deserialize_SemOutputBucket_PropriedadeFicaNula()
    {
        var json = """{"framesBucket": "x", "framesBasePrefix": "y"}""";
        var result = JsonSerializer.Deserialize<FinalizerInput>(json, JsonOptions);
        result!.OutputBucket.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Deserialize_SemOrdenaAutomaticamente_DefaultTrue()
    {
        var json = """{"framesBucket": "b", "framesBasePrefix": "p/", "outputBucket": "o", "videoId": "v", "outputBasePrefix": "out/"}""";
        var result = JsonSerializer.Deserialize<FinalizerInput>(json, JsonOptions);
        result!.OrdenaAutomaticamente.Should().BeTrue();
    }

    [Fact]
    public void Deserialize_ComOrdenaAutomaticamenteFalse_RetornaFalse()
    {
        var json = """{"framesBucket": "b", "framesBasePrefix": "p/", "outputBucket": "o", "videoId": "v", "outputBasePrefix": "out/", "ordenaAutomaticamente": false}""";
        var result = JsonSerializer.Deserialize<FinalizerInput>(json, JsonOptions);
        result!.OrdenaAutomaticamente.Should().BeFalse();
    }
}
