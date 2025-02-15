using System;
using System.Threading.Tasks;

namespace Tx.Core.Pipeline;

public static class AsyncPipelineStepExtensions
{
    public static async Task<TOut> Step<TIn, TOut>(this Task<TIn> input, IAsyncPipelineStep<TIn, TOut> step)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (step == null) throw new ArgumentNullException(nameof(step));

        var inputTask = await input;
        return await step.ProcessAsync(inputTask);
    }

    public static async Task<TOut> Step<TIn, TOut>(this TIn input, IAsyncPipelineStep<TIn, TOut> step)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (step == null) throw new ArgumentNullException(nameof(step));

        return await step.ProcessAsync(input);
    }

    public static async Task<TOut> When<TIn, TOut>(this Task<TIn> input, Func<bool> condition, IAsyncPipelineStep<TIn, TOut> step, IAsyncPipelineStep<TIn, TOut> elseStep)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (step == null) throw new ArgumentNullException(nameof(step));
        if (elseStep == null) throw new ArgumentNullException(nameof(elseStep));

        var inputTask = await input;
        var stepTo = condition() ? step.ProcessAsync(inputTask) : elseStep.ProcessAsync(inputTask);
        return await stepTo;
    }
}