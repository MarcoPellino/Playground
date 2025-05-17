using RateTablesSample.Interfaces;

namespace RateTablesSample.Models.Rules.Base
{
    public class NotRuleSpecification<T>(IRuleSpecification<T> specification) : IRuleSpecification<T>
    {
        public bool IsSatisfiedBy(T obj)
        {
            return !specification.IsSatisfiedBy(obj);
        }
    }
}