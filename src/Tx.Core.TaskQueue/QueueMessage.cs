using System;

namespace Tx.Core.TaskQueue;

public class QueueMessage<T>
{
    public T PayLoad;
    public DateTime Created { get; set; } = DateTime.Now;

    public QueueMessage(T payLoad)
    {
        PayLoad = payLoad;
    }
}