namespace Tx.Core.RabbitMqProvider.Provider
{
    public interface IMessageQueueProvider
    {
        void SendMessage<T>(string queueName, T message);
        T ReceiveMessage<T>(string queueName);
    }

}
