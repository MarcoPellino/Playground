using RateTablesSample.Interfaces;

namespace RateTablesSample.Models.Rules.Base
{
    public static class RuleSpecificationExtensions
    {
        public static IRuleSpecification<T> And<T>(this IRuleSpecification<T> first, IRuleSpecification<T> second)
        {
            return new AndRuleSpecification<T>(first, second);
        }

        public static IRuleSpecification<T> Or<T>(this IRuleSpecification<T> first, IRuleSpecification<T> second)
        {
            return new OrRuleSpecification<T>(first, second);
        }

        public static IRuleSpecification<T> Not<T>(this IRuleSpecification<T> specification)
        {
            return new NotRuleSpecification<T>(specification);
        }
    }
}