namespace RateTablesSample.Interfaces
{
    public interface IRuleSpecification<T>
    {
        bool IsSatisfiedBy(T obj);
    }
}