using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;


namespace Tx.Core.Azure.ServiceBus.Plugins;

public class MessageAddUserPropertiesPlugin : ServiceBusPlugin
{
    private readonly Dictionary<string, object> _userProperties;

    public override string Name => "Microsoft.Azure.ServiceBus.MessageUserProperties";

    
    public MessageAddUserPropertiesPlugin(Dictionary<string, object> userProperties)
    {
        _userProperties = userProperties ?? throw new ArgumentNullException(nameof(userProperties));
    }

    public override Task<Message> BeforeMessageSend(Message message)
    {
        if (!string.IsNullOrEmpty(message.MessageId)) { return base.BeforeMessageSend(message); }
        
        foreach (var prop in _userProperties)
        {
            message.UserProperties.Add(prop.Key, prop.Value);
        }

        return Task.FromResult(message);
    }
}