using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Tx.Core.Azure.ServiceBus.Plugins;

public abstract class ServiceBusPlugin
{
    public abstract string Name { get; }
    public virtual bool ShouldContinueOnException => false;
    public virtual Task<Message> BeforeMessageSend(Message message)
    {
        return Task.FromResult(message);
    }
    public virtual Task<Message> AfterMessageReceive(Message message)
    {
        return Task.FromResult(message);
    }
}