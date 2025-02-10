using System.Threading.Tasks;

namespace Tx.Core.Pipeline;

public interface IAsyncPipelineStep<in TIn, TOut>
{
    string StepName { get; }

    Task<TOut> ProcessAsync(TIn input);
}