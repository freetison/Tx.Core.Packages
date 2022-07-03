using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Tx.Core.Azure.ServiceBus.Plugins;

public class MessageAddLabelPlugin : ServiceBusPlugin
{
    private readonly string _label;

    public override string Name => "Microsoft.Azure.ServiceBus.MessageLabel";


    public MessageAddLabelPlugin(string label) => _label = label ?? throw new ArgumentNullException(nameof(label));

    public override Task<Message> BeforeMessageSend(Message message)
    {
        if (!string.IsNullOrEmpty(message.MessageId)) { return base.BeforeMessageSend(message); }

        message.Label = _label;

        return Task.FromResult(message);
    }
}