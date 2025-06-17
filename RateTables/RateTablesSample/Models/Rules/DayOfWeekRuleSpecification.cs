using RateTablesSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.Models.Rules
{
    public class DayOfWeekRuleSpecification : IRuleSpecification<Travel>
    {
        private readonly DayOfWeek dayOfWeek;
        public DayOfWeekRuleSpecification(DayOfWeek dayOfWeek)
        {
            this.dayOfWeek = dayOfWeek;
        }

        public bool IsSatisfiedBy(Travel travel)
        {
            return travel.Date.DayOfWeek == dayOfWeek;
        }
    }
}
