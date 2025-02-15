using Tx.Core.Extensions.String;

namespace Tx.Core.RabbitMqProvider
{
    using Microsoft.Extensions.Logging;

    using RabbitMQ.Client;

    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="RabbitMqClientProvider" />.
    /// </summary>
    public class RabbitMqClientProvider : IRabbitMqClientProvider
    {
        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<RabbitMqClientProvider> _logger;

        /// <summary>
        /// Defines the _channel.
        /// </summary>
        private readonly IChannel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqClientProvider"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{RabbitMqClientProvider}"/>.</param>
        /// <param name="channel">The channel<see cref="IModel"/>.</param>
        public RabbitMqClientProvider(ILogger<RabbitMqClientProvider> logger, IChannel channel)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// The PublishMessage.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="message">The message T.</param>
        /// <param name="exchangeName">The exchangeName<see cref="string"/>.</param>
        /// <param name="routingKey">The routingKey<see cref="string"/>.</param>
        /// <param name="props"></param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task PublishMessage<T>(T message, string exchangeName, string routingKey, BasicProperties? props = null)
        {
            try
            {
                if (message == null) throw new ArgumentNullException(nameof(message));
                _logger.LogDebug("Publishing message to Exchange: {ExchangeName} with Routing Key: {RoutingKey}", exchangeName, routingKey);

                var body = Encoding.UTF8.GetBytes(message.ToJson());

                await _channel.QueueDeclareAsync(queue: routingKey, durable: true, exclusive: false, autoDelete: false, arguments: null);
                await _channel.QueueBindAsync(queue: routingKey, exchange: exchangeName, routingKey: routingKey);

                await _channel.BasicPublishAsync(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: props!,
                    body: body);

                _logger.LogInformation($"Published Message to Exchange: {exchangeName} with Routing Key: {routingKey}. Message: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish message to Exchange: {exchangeName} with Routing Key: {routingKey}");
                throw;
            }
        }

        public async Task<T?> ConsumeAsync<T>(string queue, Func<ReadOnlyMemory<byte>, Task> handleMessage, TaskCompletionSource<T?> tcs)
        {
            BasicGetResult? result = null;
            try
            {
                result = await _channel.BasicGetAsync(queue, false, CancellationToken.None);
                if (result == null)
                {
                    tcs.SetResult(default);
                    return default;
                }

                await handleMessage(result.Body);
                tcs.SetResult((T)(object)Encoding.UTF8.GetString(result.Body.ToArray()));
                await _channel.BasicAckAsync(result.DeliveryTag, false, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from {Queue}", queue);
                if (result != null)
                {
                    await _channel.BasicNackAsync(result.DeliveryTag, false, true, CancellationToken.None);
                }
                tcs.SetResult(default);
            }

            return await tcs.Task;
        }

        //public async Task<T?> ConsumeAsync<T>(string queue, Func<ReadOnlyMemory<byte>, Task> handleMessage, TaskCompletionSource<T?> tcs)
        //{
        //    if (handleMessage == null) throw new ArgumentNullException(nameof(handleMessage));

        //    var result = await _channel.BasicGetAsync(queue, false);
        //    if (result == null) return default;

        //    try
        //    {
        //        await handleMessage(result.Body);
        //        await _channel.BasicAckAsync(result.DeliveryTag, false);
        //        tcs.SetResult(JsonSerializer.Deserialize<T>(result.Body.Span));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing message from {Queue}", queue);
        //        await _channel.BasicNackAsync(result.DeliveryTag, false, true);
        //        tcs.SetResult(default);
        //    }

        //    return await tcs.Task;
        //}

        public async ValueTask DisposeAsync()
        {
            _logger.LogInformation("Disposing RabbitMQ Client Provider.");
            await _channel.CloseAsync();
            _channel.Dispose();
        }
    }
}