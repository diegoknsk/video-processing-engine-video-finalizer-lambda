using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using VideoProcessing.Finalizer;
using Xunit;

namespace VideoProcessing.Finalizer.Tests;

public class FunctionTests
{
    [Fact]
    public void FunctionHandler_WithValidInput_ReturnsExpectedMessage()
    {
        var function = new Function();
        var context = new TestLambdaContext();

        var result = function.FunctionHandler("{\"message\":\"test\"}", context);

        result.Should().NotBeNull();
        result.Message.Should().Be("Hello World from Finalizer");
        result.Input.Should().Be("{\"message\":\"test\"}");
    }

    [Fact]
    public void FunctionHandler_WithNullInput_ReturnsMessageWithNullInput()
    {
        var function = new Function();
        var context = new TestLambdaContext();

        var result = function.FunctionHandler(null, context);

        result.Should().NotBeNull();
        result.Message.Should().Be("Hello World from Finalizer");
        result.Input.Should().BeNull();
    }

    [Fact]
    public void FunctionHandler_WithContext_ReturnsRequestId()
    {
        var function = new Function();
        var context = new TestLambdaContext
        {
            AwsRequestId = "test-request-id-12345"
        };

        var result = function.FunctionHandler("input", context);

        result.Should().NotBeNull();
        result.RequestId.Should().Be("test-request-id-12345");
    }

    [Fact]
    public void FunctionHandler_LogsInputAndOutput()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var testLogger = (TestLambdaLogger)context.Logger;

        function.FunctionHandler("test-input", context);

        var logOutput = testLogger.Buffer.ToString();
        logOutput.Should().Contain("test-input");
        logOutput.Should().Contain("Hello World from Finalizer");
    }
}
