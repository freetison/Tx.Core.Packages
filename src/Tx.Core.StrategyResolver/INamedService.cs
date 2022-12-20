namespace Tx.Core.StrategyResolver;

public interface INamedService<out T, in TK>
{
    string Name { get; }
    T Process(TK request);
}