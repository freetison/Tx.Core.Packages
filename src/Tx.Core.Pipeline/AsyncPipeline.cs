using System;
using System.Threading.Tasks;

namespace Tx.Core.Pipeline;

public abstract class AsyncPipeline<TIn, TOut> : IAsyncPipelineStep<TIn, TOut>
{
    public Func<TIn, Task<TOut>> PipelineSteps { get; protected set; }

    public string StepName => GetType().Name;
    public Task<TOut> ProcessAsync(TIn input) => PipelineSteps(input);
}