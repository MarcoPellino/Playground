using RateTablesSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.Models.Rules.Base
{
    public class RuleBuilder<T>
    {
        private readonly IRuleSpecification<T> current;

        public RuleBuilder(IRuleSpecification<T> rule)
        {
            current = rule;
        }

        public RuleBuilder<T> And(IRuleSpecification<T> otherRule)
        {
            return new RuleBuilder<T>(new AndRuleSpecification<T>(current, otherRule));
        }

        public RuleBuilder<T> Or(IRuleSpecification<T> other)
        {
            return new RuleBuilder<T>(new OrRuleSpecification<T>(current, other));
        }

        public IRuleSpecification<T> Build() => current;
    }
}
