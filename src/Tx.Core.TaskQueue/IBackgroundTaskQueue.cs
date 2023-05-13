using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.Core.TaskQueue;

public interface IBackgroundTaskQueue
{
    Task EnqueueTaskAsync(Func<CancellationToken, Task> workItem);

    Task<Func<CancellationToken, Task>?> DequeueAsync(CancellationToken cancellationToken);

}