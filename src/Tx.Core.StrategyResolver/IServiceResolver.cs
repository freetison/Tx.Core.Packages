namespace Tx.Core.StrategyResolver;

public interface IServiceResolver<out T, in TK>
{
    T Execute(string processorName, TK request);
}