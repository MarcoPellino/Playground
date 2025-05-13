using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.Models
{
    public class Rule
    {
        public Func<bool> ShouldApply { get; set; }
        public Modifier Modifier { get; set; }
    }
}
