namespace Tx.Core.Pipeline.Test.Pipeline
{
    using Shouldly;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class AsyncPipelineStepExtensionsTests
    {
        private class TestStep : IAsyncPipelineStep<string, int>
        {
            public string StepName => "TestStep";

            public Task<int> ProcessAsync(string input)
            {
                return Task.FromResult(input.Length);
            }
        }

        private class AlternateStep : IAsyncPipelineStep<int, int>
        {
            public string StepName => "AlternateStep";

            public Task<int> ProcessAsync(int input)
            {
                return Task.FromResult(input * 2);
            }
        }

        private readonly TestStep _testStep;
        private readonly AlternateStep _alternateStep;

        public AsyncPipelineStepExtensionsTests()
        {
            _testStep = new TestStep();
            _alternateStep = new AlternateStep();
        }

        [Fact]
        public async Task Step_WithTaskInput_ProcessesCorrectly()
        {
            // Arrange
            var input = Task.FromResult("test");

            // Act
            var result = await input.Step(_testStep);

            // Assert
            result.ShouldBe(4);
        }

        [Fact]
        public async Task Step_WithDirectInput_ProcessesCorrectly()
        {
            // Arrange
            var input = "test";

            // Act
            var result = await input.Step(_testStep);

            // Assert
            result.ShouldBe(4);
        }

        [Theory]
        [InlineData(true, 4)]  // Use primary step
        [InlineData(false, 8)] // Use alternate step
        public async Task When_ConditionDeterminesStep_ProcessesCorrectly(bool condition, int expectedResult)
        {
            // Arrange
            var input = Task.FromResult(2); // Change input to int
            bool GetCondition() => condition;

            // Act
            var result = await input.When(GetCondition, _alternateStep, _alternateStep); // Use int steps

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Fact]
        public async Task Step_WithNullInput_ThrowsException()
        {
            // Arrange
            string nullInput = null;

            // Act & Assert
            var exception = await Should.ThrowAsync<NullReferenceException>(
                async () => await nullInput.Step(_testStep));
        }

        [Fact]
        public async Task Step_WithNullTask_ThrowsException()
        {
            // Arrange
            Task<string> nullTask = null;

            // Act & Assert
            await Should.ThrowAsync<ArgumentNullException>(
                async () => await nullTask.Step(_testStep));
        }

        [Fact]
        public async Task When_WithNullCondition_ThrowsException()
        {
            // Arrange
            var input = Task.FromResult("test");
            Func<bool> nullCondition = null;

            // Act & Assert
            await Should.ThrowAsync<ArgumentNullException>(
                async () => await input.When(nullCondition, _testStep, _testStep));
        }

        [Fact]
        public async Task When_WithNullStep_ThrowsException()
        {
            // Arrange
            var input = Task.FromResult("test");

            // Act & Assert
            await Should.ThrowAsync<ArgumentNullException>(
                async () => await input.When(() => true, null, _testStep));
        }

        [Fact]
        public async Task Step_WithEmptyInput_ReturnsZero()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var result = await input.Step(_testStep);

            // Assert
            result.ShouldBe(0);
        }

        [Fact]
        public async Task Step_ChainedCalls_ProcessesCorrectly()
        {
            // Arrange
            var input = "test";

            // Act
            var result = await input
                .Step(_testStep)
                .Step(_alternateStep); // Chain multiple steps

            // Assert
            result.ShouldBe(1); // Length of "4" is 1
        }
    }
}