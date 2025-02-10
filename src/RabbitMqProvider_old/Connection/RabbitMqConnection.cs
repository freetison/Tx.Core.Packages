using RabbitMQ.Client;

using RabbitMqProvider.Models;

namespace RabbitMqProvider.Connection;

public class RabbitMqConnection : IRabbitMqConnection, IDisposable
{
    private IConnection? _connection;
    public IConnection Connection => _connection!;

    public RabbitMqConnection(RabbitMqConfigurationSettings settings) => InitConnection(settings);

    private void InitConnection(RabbitMqConfigurationSettings settings)
    {
        var factory = new ConnectionFactory
        {
            HostName = settings.RabbitMqHostname,
        };
        _connection = factory.CreateConnection();
    }

    public void Dispose() => _connection?.Dispose();
}