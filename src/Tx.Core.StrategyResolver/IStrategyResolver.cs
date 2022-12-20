namespace Tx.Core.StrategyResolver;

public interface IStrategyResolver<out T, in TK>
{
    INamedService<T, TK> Resolve(string name);
}