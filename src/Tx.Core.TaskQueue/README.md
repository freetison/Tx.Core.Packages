Basic Queue implementation

Exmaple:

1. Define one or more Pipelines, you can exchange the steps
```<language>
    Queue.EnqueueTaskAsync(async token =>
            {
                logger.LogDebug("saving");
                await Task.Delay(TimeSpan.FromSeconds(5), token);

            });

```


2.  DequeueAsync msg
```<language>
     var workItem = await TaskQueue.DequeueAsync(cancellationToken);
```

