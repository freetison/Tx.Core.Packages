namespace Tx.Core.GenericStrategy;

public interface IGenericStrategyProcessor<out T, in TK>
{
    string Name { get; }
    T Process(TK request);
}