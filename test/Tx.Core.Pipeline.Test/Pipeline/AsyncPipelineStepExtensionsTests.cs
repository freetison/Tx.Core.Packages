using Moq;

namespace Tx.Core.Pipeline.Test.Pipeline
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class AsyncPipelineStepExtensionsTests
    {
        [Fact]
        public async Task Step_InputIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Task<object> input = null;
            var step = new Mock<IAsyncPipelineStep<object, object>>().Object;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => input.Step(step));
        }

        [Fact]
        public async Task Step_StepIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var input = Task.FromResult(new object());
            IAsyncPipelineStep<object, object> step = null;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => input.Step(step));
        }

        [Fact]
        public async Task Step_ValidInputAndStep_ProcessesInput()
        {
            // Arrange
            var input = Task.FromResult("input");
            var step = new Mock<IAsyncPipelineStep<string, string>>();
            step.Setup(s => s.ProcessAsync(It.IsAny<string>())).ReturnsAsync("output");

            // Act
            var result = await input.Step(step.Object);

            // Assert
            Assert.Equal("output", result);
        }

        [Fact]
        public async Task Step_TInputIsNotTask_ValidInputAndStep_ProcessesInput()
        {
            // Arrange
            var input = "input";
            var step = new Mock<IAsyncPipelineStep<string, string>>();
            step.Setup(s => s.ProcessAsync(It.IsAny<string>())).ReturnsAsync("output");

            // Act
            var result = await input.Step(step.Object);

            // Assert
            Assert.Equal("output", result);
        }

        [Fact]
        public async Task When_InputIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Task<object> input = null;
            var step = new Mock<IAsyncPipelineStep<object, object>>().Object;
            var elseStep = new Mock<IAsyncPipelineStep<object, object>>().Object;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => input.When(Condition(true), step, elseStep));
        }

        [Fact]
        public async Task When_StepIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var input = Task.FromResult(new object());
            IAsyncPipelineStep<object, object> step = null;
            var elseStep = new Mock<IAsyncPipelineStep<object, object>>().Object;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => input.When(Condition(true), step, elseStep));
        }

        [Fact]
        public async Task When_ElseStepIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var input = Task.FromResult(new object());
            var step = new Mock<IAsyncPipelineStep<object, object>>().Object;
            IAsyncPipelineStep<object, object> elseStep = null;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncPipelineStepExtensions.When(input, () => true, step, elseStep));
        }

        [Fact]
        public async Task When_ConditionIsTrue_ExecutesStep()
        {
            // Arrange
            var input = Task.FromResult("input");
            var step = new Mock<IAsyncPipelineStep<string, string>>();
            step.Setup(s => s.ProcessAsync(It.IsAny<string>())).ReturnsAsync("output");
            var elseStep = new Mock<IAsyncPipelineStep<string, string>>();

            // Act
            var result = await input.When(Condition(true), step.Object, elseStep.Object);

            // Assert
            Assert.Equal("output", result);
        }

        [Fact]
        public async Task When_ConditionIsFalse_ExecutesElseStep()
        {
            // Arrange
            var input = Task.FromResult("input");
            var step = new Mock<IAsyncPipelineStep<string, string>>();
            var elseStep = new Mock<IAsyncPipelineStep<string, string>>();
            elseStep.Setup(s => s.ProcessAsync(It.IsAny<string>())).ReturnsAsync("output");

            // Act
            var result = await input.When(Condition(false), step.Object, elseStep.Object);

            // Assert
            Assert.Equal("output", result);
        }

        private Func<bool> Condition(bool v) => () => v;
    }
}