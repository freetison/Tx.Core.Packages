using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Tx.Core.Azure.ServiceBus.Common;

namespace Tx.Core.Azure.ServiceBus.Subscription
{
    public class AzureSubscriptionReceiver<T> : IAzureSubscriptionReceiver<T> where T : class
    {
        private readonly ISubscriptionClient _subscriptionClient;

        public AzureSubscriptionReceiver(ISubscriptionClient subscriptionClient)
        {
            _subscriptionClient = subscriptionClient;
        }

        public void Receive(
            Func<T, MessageProcessResponse> onProcess,
            Action<Exception> onError,
            Action onWait)
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

            _subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    try
                    {
                        // Get message
                        var data = Encoding.UTF8.GetString(message.Body);
                        T item = JsonConvert.DeserializeObject<T>(data);

                        // Process message
                        var result = onProcess(item);

                        if (result == MessageProcessResponse.Complete)
                            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                        else if (result == MessageProcessResponse.Abandon)
                            await _subscriptionClient.AbandonAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                        else if (result == MessageProcessResponse.Dead)
                            await _subscriptionClient.DeadLetterAsync(message.SystemProperties.LockToken).ConfigureAwait(false);

                        // Wait for next message
                        onWait();
                    }
                    catch (Exception ex)
                    {
                        await _subscriptionClient.DeadLetterAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                        onError(ex);
                    }
                }, options);
        }

    }
}
