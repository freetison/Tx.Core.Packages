namespace Tx.Core.GenericRules
{
    using System.Collections.Generic;
using Tx.Core.GenericRules;

    public class RulesBuilder<TResult, TInput>
    {
        private readonly List<IRule<TResult, TInput>> Rules = new List<IRule<TResult, TInput>>();

        public RulesBuilder<TResult, TInput> WithRule(IRule<TResult, TInput> rule)
        {
            Rules.Add(rule);
            return this;
        }
        public List<IRule<TResult, TInput>> Build() => Rules;

        public static implicit operator List<IRule<TResult, TInput>>(RulesBuilder<TResult, TInput> instance) => instance.Build();

    }
}
