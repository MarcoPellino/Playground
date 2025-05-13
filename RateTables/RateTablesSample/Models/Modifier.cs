using RateTablesSample.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.Models
{
    public class Modifier
    {
        public ModifierOperators Operator { get; set; }
        public decimal Value { get; set; }
        public bool StopApplyOthers { get; set; }
        public decimal Apply(decimal price)
        {
            switch (Operator)
            {
                case ModifierOperators.Add: return price + Value;
                case ModifierOperators.Substract: return price - Value;
                case ModifierOperators.Multiply: return price * Value;
                case ModifierOperators.Divide: return price / Value;
                case ModifierOperators.AddPercentage: return price + (price * Value / 100);
                case ModifierOperators.RemovePercentage: return price - (price * Value / 100);
                case ModifierOperators.Flat: return Value;
                default: return price;
            }
        }
    }
}
