namespace Tx.Core.RabbitMqProvider
{
    using RabbitMQ.Client;

    public interface IRabbitMqClientProvider : IAsyncDisposable
    {
        Task PublishMessage<T>(T message, string exchangeName, string routingKey, BasicProperties? props);

        Task<T?> ConsumeAsync<T>(string queue, Func<ReadOnlyMemory<byte>, Task> handleMessage, TaskCompletionSource<T?> tcs);
    }
}