using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Tx.Core.Azure.ServiceBus.Common;

namespace Tx.Core.Azure.ServiceBus.Topic
{
    public class AzureTopicSender : IAzureMessageSender, IAzureTopicPartitionedMessageSender
    {
        private readonly ITopicClient _client;

        public AzureTopicSender(ITopicClient client)
        {
            _client = client;
        }


        public async Task SendAsync(Message message, Dictionary<string, object> properties)
        {
            message = AddProperties(message, properties);
            await _client.SendAsync(message);
        }

        public async Task SendAsync(Message message) => await _client.SendAsync(message);

        public async Task SendAsync(Message message, Dictionary<string, object> properties, string partitionKey)
        {
            message = AddProperties(message, properties);
            message.PartitionKey = partitionKey;
            await _client.SendAsync(message);
        }

        public async Task SendAsync(Message message, string partitionKey)
        {
            message.PartitionKey = partitionKey;
            await _client.SendAsync(message);
        }

        private Message AddProperties(Message message, Dictionary<string, object> properties)
        {
            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    message.UserProperties.Add(prop.Key, prop.Value);
                }
            }

            return message;
        }
    }
}
