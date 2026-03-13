using System.Text;
using System.Text.Json;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using NSubstitute;
using VideoProcessing.Finalizer.Lambda;
using VideoProcessing.Finalizer.Application.Ports;
using Xunit;

namespace VideoProcessing.Finalizer.Tests;

public class FunctionTests
{
    private static JsonElement JsonToElement(string json)
    {
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.Clone();
    }

    private const string PayloadValido = """{"framesBucket": "b1", "framesBasePrefix": "p/job1", "outputBucket": "b2", "videoId": "vid-1", "outputBasePrefix": "usr123/vid-1"}""";

    [Fact]
    public async Task FunctionHandler_ComPayloadDiretoValido_RetornaResultadoComChaveEFramesCount()
    {
        var service = Substitute.For<IFramesZipService>();
        service.ListAllFrameKeysAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<string> { "p/job1/chunk-0/frames/f1.jpg" });
        service.DownloadFramesAsync(Arg.Any<string>(), Arg.Any<IEnumerable<string>>(), Arg.Any<string>())
            .Returns(Task.FromResult(Path.Combine(Path.GetTempPath(), "frames")));
        service.UploadZipToS3Async(Arg.Any<string>(), Arg.Any<string>(), Arg.Is<string>(k => k == "usr123/vid-1/vid-1_frames.zip"))
            .Returns(Task.FromResult("usr123/vid-1/vid-1_frames.zip"));
        var function = new Function(service);
        var context = new TestLambdaContext();

        var result = await function.FunctionHandler(JsonToElement(PayloadValido), context);

        result.ZipBucket.Should().Be("b2");
        result.ZipS3Key.Should().Be("usr123/vid-1/vid-1_frames.zip");
        result.ZipS3Key.Should().EndWith("_frames.zip");
        result.FramesCount.Should().Be(1);
        await service.Received(1).UploadZipToS3Async(Arg.Any<string>(), Arg.Any<string>(), "usr123/vid-1/vid-1_frames.zip", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task FunctionHandler_ComFramesBucketVazio_LancaArgumentException()
    {
        var json = """{"framesBucket": "", "framesBasePrefix": "p/job1", "outputBucket": "b2", "videoId": "v1", "outputBasePrefix": "u/v"}""";
        var function = new Function(Substitute.For<IFramesZipService>());
        var context = new TestLambdaContext();

        var act = () => function.FunctionHandler(JsonToElement(json), context);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*framesBucket*");
    }

    [Fact]
    public async Task FunctionHandler_ComFramesBasePrefixVazio_LancaArgumentException()
    {
        var json = """{"framesBucket": "b1", "framesBasePrefix": "", "outputBucket": "b2", "videoId": "v1", "outputBasePrefix": "u/v"}""";
        var function = new Function(Substitute.For<IFramesZipService>());
        var context = new TestLambdaContext();

        var act = () => function.FunctionHandler(JsonToElement(json), context);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*framesBasePrefix*");
    }

    [Fact]
    public async Task FunctionHandler_ComOutputBucketVazio_LancaArgumentException()
    {
        var json = """{"framesBucket": "b1", "framesBasePrefix": "p/job1", "outputBucket": "", "videoId": "v1", "outputBasePrefix": "u/v"}""";
        var function = new Function(Substitute.For<IFramesZipService>());
        var context = new TestLambdaContext();

        var act = () => function.FunctionHandler(JsonToElement(json), context);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*outputBucket*");
    }

    [Fact]
    public async Task FunctionHandler_ComVideoIdVazio_LancaArgumentException()
    {
        var json = """{"framesBucket": "b1", "framesBasePrefix": "p/job1", "outputBucket": "b2", "videoId": "", "outputBasePrefix": "u/v"}""";
        var function = new Function(Substitute.For<IFramesZipService>());
        var context = new TestLambdaContext();

        var act = () => function.FunctionHandler(JsonToElement(json), context);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*videoId*");
    }

    [Fact]
    public async Task FunctionHandler_ComOutputBasePrefixVazio_LancaArgumentException()
    {
        var json = """{"framesBucket": "b1", "framesBasePrefix": "p/job1", "outputBucket": "b2", "videoId": "v1", "outputBasePrefix": ""}""";
        var function = new Function(Substitute.For<IFramesZipService>());
        var context = new TestLambdaContext();

        var act = () => function.FunctionHandler(JsonToElement(json), context);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*outputBasePrefix*");
    }

    [Fact]
    public async Task FunctionHandler_ComEnvelopeSqs_DeserializaBodyERetornaResultado()
    {
        var bodyContent = new { framesBucket = "b1", framesBasePrefix = "p/job1", outputBucket = "b2", videoId = "vid-1", outputBasePrefix = "usr123/vid-1" };
        var bodyJson = JsonSerializer.Serialize(bodyContent);
        var sqsEnvelope = new { Records = new[] { new { body = bodyJson } } };
        var sqsJson = JsonSerializer.Serialize(sqsEnvelope);
        var service = Substitute.For<IFramesZipService>();
        service.ListAllFrameKeysAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<string> { "p/job1/chunk-0/frames/f1.jpg" });
        service.DownloadFramesAsync(Arg.Any<string>(), Arg.Any<IEnumerable<string>>(), Arg.Any<string>())
            .Returns(Task.FromResult(Path.Combine(Path.GetTempPath(), "frames")));
        service.UploadZipToS3Async(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.FromResult("usr123/vid-1/vid-1_frames.zip"));
        var function = new Function(service);
        var context = new TestLambdaContext();

        var result = await function.FunctionHandler(JsonToElement(sqsJson), context);

        result.ZipBucket.Should().Be("b2");
        result.FramesCount.Should().Be(1);
    }

    [Fact]
    public async Task FunctionHandler_QuandoServicoLancaExcecao_PropagaExcecao()
    {
        var service = Substitute.For<IFramesZipService>();
        service.ListAllFrameKeysAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.FromException<IReadOnlyList<string>>(new InvalidOperationException("Nenhum frame encontrado no prefixo informado.")));
        var function = new Function(service);
        var context = new TestLambdaContext();

        var act = () => function.FunctionHandler(JsonToElement(PayloadValido), context);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Nenhum frame encontrado*");
    }
}
