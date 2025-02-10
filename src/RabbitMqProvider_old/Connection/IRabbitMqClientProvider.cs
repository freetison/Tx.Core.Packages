namespace RabbitMqProvider.Connection;

public interface IRabbitMqClientProvider
{
    void PublishMessage(string message, string queueName);
    void ReadMessages(string queueName, Action<string> processMessage);
}

