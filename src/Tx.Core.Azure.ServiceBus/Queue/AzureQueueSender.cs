using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Tx.Core.Azure.ServiceBus.Common;

namespace Tx.Core.Azure.ServiceBus.Queue
{
    public class AzureQueueSender : IAzureMessageSender
    {

        private readonly IQueueClient _queueClient;

        public AzureQueueSender(IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }


        public async Task SendAsync(Message message) => await SendAsync(message, null).ConfigureAwait(false);

        public async Task SendAsync(Message message, Dictionary<string, object> properties)
        {
            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    message.UserProperties.Add(prop.Key, prop.Value);
                }
            }

            await _queueClient.SendAsync(message).ConfigureAwait(false);
        }
    }
}
