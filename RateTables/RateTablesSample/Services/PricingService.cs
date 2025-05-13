using RateTablesSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateTablesSample.BOL
{
    public class PricingService
    {
        public decimal GetPrice(Travel travel, List<Rule> travelRules)
        {
            decimal price = travel.BasePrice;

            foreach (var rule in travelRules) 
            {
                if(rule.ShouldApply(travel)) 
                {
                    // applico la regola
                    price = rule.Modifier.Apply(price);

                    // se il modificatore blocca l'applicazione di altri modificatori esco dal ciclo
                    if(rule.Modifier.StopApplyOthers)
                    {
                        break;
                    }
                }
            }

            return price;
        }
    }
}
