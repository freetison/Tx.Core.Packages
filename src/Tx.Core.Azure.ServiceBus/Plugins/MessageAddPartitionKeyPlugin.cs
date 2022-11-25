using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Tx.Core.Azure.ServiceBus.Plugins;

public class MessageAddPartitionKeyPlugin : ServiceBusPlugin
{
    private readonly string _partitionKey;

    public override string Name => "Microsoft.Azure.ServiceBus.MessagePartitionKey";


    public MessageAddPartitionKeyPlugin(string partitionKey) => _partitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));

    public override Task<Message> BeforeMessageSend(Message message)
    {
        if (!string.IsNullOrEmpty(message.MessageId)) { return base.BeforeMessageSend(message); }

        message.PartitionKey = _partitionKey;

        return Task.FromResult(message);
    }
}