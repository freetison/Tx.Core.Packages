using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;
using Tx.Core.TaskQueue;

namespace Tx.Core.QueuedHostedService;

public class QueuedHostedService : BackgroundService
{
    private readonly ILogger<QueuedHostedService> _logger;

    public QueuedHostedService(IBackgroundTaskQueue taskQueue, ILogger<QueuedHostedService> logger)
    {
        TaskQueue = taskQueue;
        _logger = logger;
    }

    public IBackgroundTaskQueue TaskQueue { get; }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Queued Hosted Service is starting.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var workItem = await TaskQueue.DequeueAsync(cancellationToken);

            try
            {
                if (workItem != null) await workItem(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
            }
        }

        _logger.LogDebug("Queued Hosted Service is stopping.");
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("App stopped !!!");
        await base.StopAsync(stoppingToken);
    }
}