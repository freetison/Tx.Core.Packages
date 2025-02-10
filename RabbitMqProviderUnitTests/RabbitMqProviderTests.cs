using Moq;

using RabbitMQ.Client;

using System.Text;

namespace RabbitMqProviderUnitTests
{
    public class RabbitMqProviderTests
    {
        private readonly Mock<IConnection> _mockConnection;
        private readonly Mock<IModel> _mockChannel;

        public RabbitMqProviderTests()
        {
            _mockConnection = new Mock<IConnection>();
            _mockChannel = new Mock<IModel>();

            _mockConnection.Setup(c => c.CreateModel()).Returns(_mockChannel.Object);
        }

        [Fact]
        public void SendMessage_Should_SendMessageToQueue()
        {
            // Arrange
            var provider = new Tx.Core.RabbitMqProvider.Provider.RabbitMqProvider("localhost", "guest", "guest");

            // Act
            provider.SendMessage("testQueue", "Test Message");

            // Assert
            _mockChannel.Verify(c => c.BasicPublish(It.IsAny<string>(), "testQueue", It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void ReceiveMessage_Should_ReturnMessageFromQueue()
        {
            // Arrange
            var provider = new Tx.Core.RabbitMqProvider.Provider.RabbitMqProvider("localhost", "guest", "guest");

            // Mock the message content
            var body = Encoding.UTF8.GetBytes("Test Message");

            // Mock the BasicGetResult
            _mockChannel.Setup(c => c.BasicGet("testQueue", It.IsAny<bool>())).Returns(new BasicGetResult(1, false, "exchange", "routingKey", 1, null, body));

            // Act
            var result = provider.ReceiveMessage<string>("testQueue");

            // Assert
             ("Test Message", result);
        }
    }
}