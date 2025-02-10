using System;

namespace RabbitMqProvider.Provider.RabbitMq;

public class RabbitMqMessage<TBody> : IRabbitMqMessage
{
    public Guid MessageId { get; set; }
    public TimeSpan TimeToLive { get; set; }
    public required TBody Body { get; init; }
    Guid IRabbitMqMessage.MessageId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    TimeSpan IRabbitMqMessage.TimeToLive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
