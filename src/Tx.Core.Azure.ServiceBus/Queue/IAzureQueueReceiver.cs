using System;
using Tx.Core.Azure.ServiceBus.Common;

namespace Tx.Core.Azure.ServiceBus.Queue
{
    public interface IAzureQueueReceiver
    {
        void Receive<T>(Func<T, MessageProcessResponse> onProcess, Action<Exception> onError, Action onWait);
    }
}
