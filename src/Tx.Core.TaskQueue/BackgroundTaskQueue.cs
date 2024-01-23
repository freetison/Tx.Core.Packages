using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.Core.TaskQueue;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new();
    private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);


    public Task EnqueueTaskAsync(Func<CancellationToken, Task> workItem)
    {
        if (workItem == null) { throw new ArgumentNullException(nameof(workItem)); }

        return Task.Run(() =>
        {
            _workItems.Enqueue(workItem);
            _signal.Release();
        });
    }

    public async Task<Func<CancellationToken, Task>?> DequeueAsync(CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken).ConfigureAwait(false);
        _workItems.TryDequeue(out var workItem);

        return workItem;
    }
}