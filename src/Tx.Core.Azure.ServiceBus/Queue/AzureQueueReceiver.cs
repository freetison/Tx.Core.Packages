using Azure.Messaging.ServiceBus;

using Newtonsoft.Json;

using System;
using System.Text;
using System.Threading.Tasks;

using Tx.Core.Azure.ServiceBus.Common;

namespace Tx.Core.Azure.ServiceBus.Queue
{
    public class AzureQueueReceiver : IAzureQueueReceiver
    {
        private readonly ServiceBusClient _serviceBusClient;

        public AzureQueueReceiver(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        public void Receive<T>(Func<T, MessageProcessResponse> onProcess, Action<Exception> onError, Action onWait)
        {
            var options = new MessageHandlerOptions(e =>
            {
                onError(e.Exception);
                return Task.CompletedTask;
            })
            {
                AutoComplete = false,
                MaxAutoRenewDuration = TimeSpan.FromMinutes(1)
            };

            _queueClient.RegisterMessageHandler(async (message, token) =>
                {
                    try
                    {
                        // Get message
                        var data = Encoding.UTF8.GetString(message.Body);
                        T item = JsonConvert.DeserializeObject<T>(data);

                        // Process message
                        var result = onProcess(item);

                        switch (result)
                        {
                            case MessageProcessResponse.Complete:
                                await _queueClient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                                break;

                            case MessageProcessResponse.Abandon:
                                await _queueClient.AbandonAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                                break;

                            case MessageProcessResponse.Dead:
                                await _queueClient.DeadLetterAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                                break;
                        }

                        // Wait for next message
                        onWait();
                    }
                    catch (Exception ex)
                    {
                        await _queueClient.DeadLetterAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                        onError(ex);
                    }
                }, options);
        }
    }
}