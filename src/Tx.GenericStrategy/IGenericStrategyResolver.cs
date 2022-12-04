namespace Tx.Core.GenericStrategy;

public interface IGenericStrategyResolver<out T, in TK>
{
    IGenericStrategyProcessor<T, TK> Resolve(string name);
}