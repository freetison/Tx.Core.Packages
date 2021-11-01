namespace Tx.Core.GenericStrategy
{
    public interface IStrategyFactory<T, in TK>
    {
        IConcreteStrategy<T, TK> GetConcreteStrategy(TK context);
    }
}
