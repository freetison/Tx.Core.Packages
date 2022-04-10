using System;
using Tx.Core.Azure.ServiceBus.Common;

namespace Tx.Core.Azure.ServiceBus.Subscription
{
    public interface IAzureSubscriptionReceiver<out T>
    {
        void Receive(
            Func<T, MessageProcessResponse> onProcess,
            Action<Exception> onError,
            Action onWait);
    }
}
