using Microsoft.Extensions.Logging;

using Moq;

using RabbitMQ.Client;

using System.Text;

using Tx.Core.RabbitMqProvider;

namespace RabbitMqProviderUnitTests
{
    public class RabbitMqClientProviderTests
    {
        private readonly Mock<IChannel> _channelMock;
        private readonly Mock<ILogger<RabbitMqClientProvider>> _loggerMock;
        private readonly RabbitMqClientProvider _provider;

        public RabbitMqClientProviderTests()
        {
            _channelMock = new Mock<IChannel>();
            _loggerMock = new Mock<ILogger<RabbitMqClientProvider>>();
            _provider = new RabbitMqClientProvider(_loggerMock.Object, _channelMock.Object);
        }

        [Fact]
        public async Task ConsumeAsync_ValidQueue_ConsumesMessage()
        {
            // Arrange
            var queue = "my_queue";
            var message = "test message";
            var body = Encoding.UTF8.GetBytes(message);
            var basicGetResult = new BasicGetResult(1, false, "exchange", "routingKey", 1, new BasicProperties(), body);
            _channelMock.Setup(c => c.BasicGetAsync(queue, It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(basicGetResult);
            var tcs = new TaskCompletionSource<string>();

            // Act
            var result = await _provider.ConsumeAsync(queue, async (msg) => await Task.CompletedTask, tcs);

            // Assert
            Assert.Equal(message, result);
            _channelMock.Verify(c => c.BasicAckAsync(It.IsAny<ulong>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ConsumeAsync_NullMessage_ReturnsDefault()
        {
            // Arrange
            var queue = "my_queue";
            _channelMock.Setup(c => c.BasicGetAsync(queue, false, It.IsAny<CancellationToken>())).ReturnsAsync((BasicGetResult?)null);
            var tcs = new TaskCompletionSource<string>();

            // Act
            var result = await _provider.ConsumeAsync(queue, async (msg) => await Task.CompletedTask, tcs);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ConsumeAsync_HandleMessageThrowsException_NacksMessage()
        {
            // Arrange
            var queue = "my_queue";
            var message = "test message";
            var body = Encoding.UTF8.GetBytes(message);
            var basicProperties = new BasicProperties();
            var basicGetResult = new BasicGetResult(1, false, "exchange", "routingKey", 0, basicProperties, body);
            _channelMock.Setup(c => c.BasicGetAsync(queue, false, It.IsAny<CancellationToken>())).ReturnsAsync(basicGetResult);
            var tcs = new TaskCompletionSource<string?>();

            // Act
            var result = await _provider.ConsumeAsync(queue, async (msg) => await Task.FromException(new Exception("Test exception")), tcs);

            // Assert
            Assert.Null(result);
            _channelMock.Verify(c => c.BasicNackAsync(basicGetResult.DeliveryTag, false, true, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}