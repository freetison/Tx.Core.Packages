using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Tx.Core.Azure.ServiceBus.Topic
{
    public interface IAzureTopicPartitionedMessageSender
    {
        Task SendAsync(Message message, Dictionary<string, object> properties, string partitionKey);
        Task SendAsync(Message message, string partitionKey);
    }
}
