namespace RabbitMqProvider.Producer;

public interface IMessageProducer
{
    Task SendMessageAsync<T>(T message,  string toQueue);
}