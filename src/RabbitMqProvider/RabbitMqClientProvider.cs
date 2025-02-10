namespace TvMaze.RabbitMqProvider
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    using Tx.Core.Extensions.String;

    /// <summary>
    /// Defines the <see cref="RabbitMqClientProvider" />.
    /// </summary>
    public class RabbitMqClientProvider : IRabbitMqClientProvider, IDisposable
    {
        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<RabbitMqClientProvider> _logger;

        /// <summary>
        /// Defines the _channel.
        /// </summary>
        private readonly IModel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqClientProvider"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{RabbitMqClientProvider}"/>.</param>
        /// <param name="channel">The channel<see cref="IModel"/>.</param>
        public RabbitMqClientProvider(ILogger<RabbitMqClientProvider> logger, IModel channel)
        {
            _logger = logger;
            _channel = channel;
        }

        /// <summary>
        /// The ReceiveMessage.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="queueName">The queueName<see cref="string"/>.</param>
        /// <returns>The Task.</returns>
        public async Task<T?> ReceiveMessage<T>(string queueName)
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            TaskCompletionSource<T?> tcs = new();

            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var parsedMessage = message.ParseTo<T?>();

                tcs.SetResult(parsedMessage);
                await Task.Yield();
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            // Wait for the message to be received
            return await tcs.Task;
        }

        /// <summary>
        /// The PublishMessage.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="message">The message T.</param>
        /// <param name="exchangeName">The exchangeName<see cref="string"/>.</param>
        /// <param name="routingKey">The routingKey<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task PublishMessage<T>(T message, string exchangeName, string routingKey)
        {
            try
            {
                _logger.LogDebug("Publishing message to Exchange: {ExchangeName} with Routing Key: {RoutingKey}", exchangeName, routingKey);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                _channel.QueueDeclare(queue: routingKey, durable: true, exclusive: false, autoDelete: false, arguments: null);
                _channel.QueueBind(queue: routingKey, exchange: exchangeName, routingKey: routingKey);

                _channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    basicProperties: null,
                    body: body);

                _logger.LogInformation($"Published Message to Exchange: {exchangeName} with Routing Key: {routingKey}. Message: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish message to Exchange: {exchangeName} with Routing Key: {routingKey}");
                throw;
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose() => _channel?.Dispose();
    }
}