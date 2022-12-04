using System.Threading.Tasks;

namespace Tx.Core.DI.Multiple.Instance;

public interface IAsyncPipelineStep<in TIn, TOut>
{
    string StepName { get; }
    Task<TOut> ProcessAsync(TIn input);
}

