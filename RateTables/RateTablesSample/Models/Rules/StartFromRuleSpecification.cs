using RateTablesSample.Interfaces;

namespace RateTablesSample.Models.Rules
{
    public class StartFromRuleSpecification : IRuleSpecification<Travel>
    {
        private readonly string startLocation;
        public StartFromRuleSpecification(string startLocation)
        {
            this.startLocation = startLocation;
        }

        public bool IsSatisfiedBy(Travel travel)
        {
            return travel.StartLocation == startLocation;
        }
    }
}