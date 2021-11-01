namespace Tx.Core.GenericRules
{
    public interface IRule<TResult, in TInput>
    {
        TResult LastResult { get; set; }
        bool IsApplicable(TInput context);
        TResult Execute(TInput context);
    }
}
