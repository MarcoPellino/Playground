using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.Models
{
    public class Rule
    {
        public string? StartLocationMatch { get; set; }
        public string? EndLocationMatch { get; set; }
        public DayOfWeek? DayOfWeekMatch { get; set; }

        public bool ShouldApply(Travel travel)
        {
            if (StartLocationMatch != null && !travel.StartLocation.Equals(StartLocationMatch, StringComparison.OrdinalIgnoreCase))
                return false;

            if (EndLocationMatch != null && !travel.EndLocation.Equals(EndLocationMatch, StringComparison.OrdinalIgnoreCase))
                return false;

            if (DayOfWeekMatch != null && travel.Date.DayOfWeek != DayOfWeekMatch)
                return false;

            return true;
        }

        public Modifier Modifier { get; set; }
    }
}
