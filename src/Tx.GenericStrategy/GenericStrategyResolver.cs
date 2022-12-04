using System;
using System.Collections.Generic;
using System.Linq;

namespace Tx.Core.GenericStrategy;

public class GenericStrategyResolver<T, TK> : IGenericStrategyResolver<T, TK>
{
    private readonly IEnumerable<IGenericStrategyProcessor<T, TK>> _processor;

    public GenericStrategyResolver(IEnumerable<IGenericStrategyProcessor<T, TK>> processor) => _processor = processor;

    public IGenericStrategyProcessor<T, TK> Resolve(string name)
    {
        IGenericStrategyProcessor<T, TK> strategyProcessor = _processor.FirstOrDefault(item => item.Name == name);
        if (strategyProcessor == null) { throw new ArgumentException("Payment method not found", name); }
        return strategyProcessor;
    }
        
}