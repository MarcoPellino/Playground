using RateTablesSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.Models.Rules.Base
{
    public class TrueRuleSpecification : IRuleSpecification<Travel>
    {
        public bool IsSatisfiedBy(Travel travel)
        {
            return true;
        }
    }
}
