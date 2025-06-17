using RateTablesSample.Interfaces;

namespace RateTablesSample.Models.Rules.Base
{
    public class OrRuleSpecification<T>(IRuleSpecification<T> first, IRuleSpecification<T> second) : IRuleSpecification<T>
    {
        public bool IsSatisfiedBy(T obj)
        {
            return first.IsSatisfiedBy(obj) || second.IsSatisfiedBy(obj);
        }
    }
}