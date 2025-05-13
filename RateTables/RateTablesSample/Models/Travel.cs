using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.Models
{
    public class Travel
    {
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public DateTime Date { get; set; }
        public decimal BasePrice { get; set; }
    }
}
