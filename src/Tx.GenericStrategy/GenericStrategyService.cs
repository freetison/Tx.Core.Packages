namespace Tx.Core.GenericStrategy;

public class GenericStrategyService<T, TK> : IGenericStrategyService<T, TK>
{
    private readonly IGenericStrategyResolver<T, TK> _strategyResolver;

    public GenericStrategyService(IGenericStrategyResolver<T, TK> strategyResolver)
    {
        _strategyResolver = strategyResolver;
    }

    public T Execute(string processorName, TK request)
    {
        IGenericStrategyProcessor<T, TK> strategyProcessor = _strategyResolver.Resolve(processorName);
        return strategyProcessor.Process(request);

    }
}