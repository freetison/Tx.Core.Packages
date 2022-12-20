namespace Tx.Core.StrategyResolver;

public class ServiceResolver<T, TK> : IServiceResolver<T, TK>
{
    private readonly IStrategyResolver<T, TK> _strategyResolver;

    public ServiceResolver(IStrategyResolver<T, TK> strategyResolver) => _strategyResolver = strategyResolver;

    public T Execute(string paymentMethodName, TK request)
    {
        INamedService<T, TK> strategyProcessor = _strategyResolver.Resolve(paymentMethodName);
        return strategyProcessor.Process(request);

    }
}