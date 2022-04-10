using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Tx.Core.Azure.ServiceBus.Common
{
    public interface IAzureMessageSender
    {
        Task SendAsync(Message message, Dictionary<string, object> properties);
        Task SendAsync(Message message);
    }
}
