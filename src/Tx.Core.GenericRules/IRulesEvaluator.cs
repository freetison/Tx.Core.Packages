namespace Tx.Core.GenericRules
{
    using System.Collections.Generic;

    public interface IRulesEvaluator<TResult, in TInput>
    {
        List<TResult> Execute(TInput context);
    }
}
