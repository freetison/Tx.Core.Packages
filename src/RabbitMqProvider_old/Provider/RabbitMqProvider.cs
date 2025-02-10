using RabbitMQ.Client;

using System;
using System.Linq;
using System.Text;

namespace Tx.Core.RabbitMqProvider.Provider
{
    public class RabbitMqProvider : IMessageQueueProvider, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqProvider(string hostname, string username, string password)
        {
            var factory = new ConnectionFactory() { HostName = hostname, UserName = username, Password = password };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendMessage<T>(string queueName, T message)
        {
            _channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            _channel.BasicPublish(exchange: "",
                                  routingKey: queueName,
                                  basicProperties: null,
                                  body: messageBody);
        }

        public T ReceiveMessage<T>(string queueName)
        {
            _channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            T receivedMessage = default;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                receivedMessage = JsonConvert.DeserializeObject<T>(message);
            };

            _channel.BasicConsume(queue: queueName,
                                  autoAck: true,
                                  consumer: consumer);

            // Return the message or a default value if no message is received.
            return receivedMessage;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
