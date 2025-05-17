using RateTablesSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.Models
{
    public class Rule
    {
        public IRuleSpecification<Travel> ApplicationRule { get; set; }

        public bool ShouldApply(Travel travel)
        {
            return ApplicationRule.IsSatisfiedBy(travel);
        }

        public Modifier Modifier { get; set; }
    }
}
