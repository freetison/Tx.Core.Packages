namespace Tx.Core.Pipeline.Test.Pipeline
{
    using Shouldly;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class AsyncPipelineTests : IDisposable
    {
        private class TestPipeline : AsyncPipeline<string, int>
        {
            public TestPipeline()
            {
                PipelineSteps = async input =>
                {
                    await Task.Delay(10);
                    return input.Length;
                };
            }
        }

        private class ExceptionPipeline : AsyncPipeline<string, int>
        {
            public ExceptionPipeline()
            {
                PipelineSteps = _ => throw new InvalidOperationException("Test exception");
            }
        }

        private readonly TestPipeline _pipeline;
        private readonly ExceptionPipeline _exceptionPipeline;

        public AsyncPipelineTests()
        {
            _pipeline = new TestPipeline();
            _exceptionPipeline = new ExceptionPipeline();
        }

        public void Dispose()
        {
            // Cleanup code if needed
        }

        [Fact]
        public async Task ProcessAsync_ValidInput_ReturnsExpectedResult()
        {
            // Arrange
            var input = "test";
            var expectedLength = 4;

            // Act
            var result = await _pipeline.ProcessAsync(input);

            // Assert
            result.ShouldBe(expectedLength);
        }

        [Fact]
        public async Task ProcessAsync_EmptyInput_ReturnsZero()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var result = await _pipeline.ProcessAsync(input);

            // Assert
            result.ShouldBe(0);
        }

        [Fact]
        public void ProcessAsync_NullInput_ThrowsArgumentNullException()
        {
            // Arrange
            string input = null;

            // Act & Assert
            Should.Throw<NullReferenceException>(async () =>
                await _pipeline.ProcessAsync(input));
        }

        [Fact]
        public void StepName_ShouldReturnClassName()
        {
            // Act & Assert
            _pipeline.StepName.ShouldBe("TestPipeline");
        }

        [Fact]
        public async Task ProcessAsync_PipelineThrowsException_PropagatesException()
        {
            // Arrange
            var input = "test";

            // Act & Assert
            var exception = await Should.ThrowAsync<InvalidOperationException>(
                async () => await _exceptionPipeline.ProcessAsync(input));

            exception.Message.ShouldBe("Test exception");
        }
    }
}