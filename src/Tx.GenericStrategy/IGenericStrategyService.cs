namespace Tx.Core.GenericStrategy;

public interface IGenericStrategyService<out T, in TK>
{
    T Execute(string processorName, TK request);
}