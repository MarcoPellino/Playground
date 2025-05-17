using RateTablesSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.Models.Rules
{
    public class EndAtRuleSpecification : IRuleSpecification<Travel>
    {
        private readonly string endLocation;
        public EndAtRuleSpecification(string endLocation )
        {
            this.endLocation = endLocation;
        }
        public bool IsSatisfiedBy(Travel travel)
        {
            return travel.EndLocation == endLocation;
        }
    }
}
