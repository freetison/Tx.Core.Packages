using System.Text;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using RabbitMQ.Client;

namespace RabbitMqProvider
{
    /// <summary>
    /// Defines the <see cref="MessageSender" />.
    /// </summary>
    public class MessageSender : IMessageSender
    {
        /// <summary>
        /// Defines the _channel.
        /// </summary>
        private readonly IModel _channel;

        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<MessageSender> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSender"/> class.
        /// </summary>
        /// <param name="channel">The channel<see cref="IModel"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{MessageSender}"/>.</param>
        public MessageSender(IModel channel, ILogger<MessageSender> logger)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// The PublishMessage.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="message">The message/>.</param>
        /// <param name="exchangeName">The exchangeName<see cref="string"/>.</param>
        /// <param name="routingKey">The routingKey<see cref="string"/>.</param>
        public void PublishMessage<T>(T message, string exchangeName, string routingKey)
        {
            try
            {
                _logger.LogDebug("Publishing message to Exchange: {ExchangeName} with Routing Key: {RoutingKey}", exchangeName, routingKey);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                _channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    basicProperties: null,
                    body: body);

                _logger.LogInformation("Published Message to Exchange: {ExchangeName} with Routing Key: {RoutingKey}. Message: {Message}", exchangeName, routingKey, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish message to Exchange: {ExchangeName} with Routing Key: {RoutingKey}", exchangeName, routingKey);
                throw;
            }
        }
    }
}
