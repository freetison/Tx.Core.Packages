using System;
using System.Linq.Expressions;

namespace Tx.Core.GenericStrategy
{
    public interface IConcreteStrategy<T, in TK>
    {
        string SupportedMethod { get; }
        Expression<Func<T, bool>> Filter(TK context);
    }
}
