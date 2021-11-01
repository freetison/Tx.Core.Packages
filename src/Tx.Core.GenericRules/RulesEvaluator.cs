namespace Tx.Core.GenericRules
{
    using System.Collections.Generic;
    using System.Linq;
using Tx.Core.GenericRules;

    public class RulesEvaluator<TResult, TInput> : IRulesEvaluator<TResult, TInput>
    {
        private readonly List<IRule<TResult, TInput>> _rules;

        public RulesEvaluator(List<IRule<TResult, TInput>> rules) => _rules = rules;

        public List<TResult> Execute(TInput context)
        {
            List<TResult> listRulesResult = new List<TResult>();
            _rules
                .Where(rule => rule.IsApplicable(context))
                .ToList()
                .ForEach(rule =>
                {
                    var rulesResult = rule.Execute(context);
                    listRulesResult.Add(rulesResult);
                });

            return listRulesResult;
        }
    }
}


