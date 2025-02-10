namespace TvMaze.RabbitMqProvider
{
    public interface IRabbitMqClientProvider
    {
        Task PublishMessage<T>(T message, string exchangeName, string routingKey);
        Task<T?> ReceiveMessage<T>(string queueName);
    }
}
