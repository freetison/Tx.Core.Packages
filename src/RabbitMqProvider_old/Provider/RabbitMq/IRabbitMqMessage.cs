namespace RabbitMqProvider.Provider.RabbitMq;

public interface IRabbitMqMessage
{
    Guid MessageId { get; set; }
    TimeSpan TimeToLive { get; set; }
}