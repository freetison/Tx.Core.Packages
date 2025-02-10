using System.Text;
using RabbitMQ.Client;
using RabbitMqProvider.Connection;
using Tx.Core.Extensions.String;

namespace RabbitMqProvider.Producer;

public class RabbitMqProducer(IRabbitMqConnection connection) : IMessageProducer
{
    public Task SendMessageAsync<T>(T message, string toQueue)
    {
        using var channel = connection.Connection.CreateModel();
        channel.QueueDeclare(toQueue, exclusive: false);
        var body = Encoding.UTF8.GetBytes(message.ToJson());
        
        channel.BasicPublish(exchange:"", routingKey: toQueue, body: body);
        return Task.CompletedTask;
    }
}