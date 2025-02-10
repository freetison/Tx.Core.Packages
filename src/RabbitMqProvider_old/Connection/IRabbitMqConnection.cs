using RabbitMQ.Client;

namespace RabbitMqProvider.Connection;

public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}