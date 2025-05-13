using RateTablesSample.BOL;
using RateTablesSample.Constants;
using RateTablesSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RateTablesSample.Tests
{
    public class PricingTests
    {
        [Fact]
        public void GetPrice_NoRules_ReturnsBasePrice()
        {
            // Impostazioni
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Senigallia", Date = DateTime.Now, BasePrice = 50 };
            var rules = new List<Rule>();
            var pricingService = new PricingService();

            // Metodo
            var result = pricingService.GetPrice(travel, rules);

            // Test
            Assert.Equal(travel.BasePrice, result);
        }

        [Fact]
        public void GetPrice_AddModifier_ReturnsBasePricePlusValue()
        {
            var travel = new Travel { BasePrice = 100 };
            var rules = new List<Rule>
            {
                new Rule
                {
                    ShouldApply = () => true,
                    Modifier = new Modifier { Operator = ModifierOperators.Add, Value = 10 }
                }
            };

            var service = new PricingService();
            var result = service.GetPrice(travel, rules);

            Assert.Equal(110, result);
        }

        [Fact]
        public void GetPrice_SubstractModifier_ReturnsBasePriceMinusValue()
        {
            var travel = new Travel { BasePrice = 100 };
            var rules = new List<Rule>
            {
                new Rule
                {
                    ShouldApply = () => true,
                    Modifier = new Modifier { Operator = ModifierOperators.Substract, Value = 10 }
                }
            };

            var service = new PricingService();
            var result = service.GetPrice(travel, rules);

            Assert.Equal(90, result);
        }

        [Fact]
        public void GetPrice_MultiplyModifier_ReturnsBasePriceMultipiledValue()
        {
            var travel = new Travel { BasePrice = 100 };
            var rules = new List<Rule>
            {
                new Rule
                {
                    ShouldApply = () => true,
                    Modifier = new Modifier { Operator = ModifierOperators.Multiply, Value = 2 }
                }
            };

            var service = new PricingService();
            var result = service.GetPrice(travel, rules);

            Assert.Equal(200, result);
        }

        [Fact]
        public void GetPrice_DivideModifier_ReturnsBasePriceDividedByValue()
        {
            var travel = new Travel { BasePrice = 100 };
            var rules = new List<Rule>
            {
                new Rule
                {
                    ShouldApply = () => true,
                    Modifier = new Modifier { Operator = ModifierOperators.Divide, Value = 2 }
                }
            };

            var service = new PricingService();
            var result = service.GetPrice(travel, rules);

            Assert.Equal(50, result);
        }

        [Fact]
        public void GetPrice_AddPercentageModifier_ReturnsBasePricePlusPercentage()
        {
            var travel = new Travel { BasePrice = 100 };
            var rules = new List<Rule>
            {
                new Rule
                {
                    ShouldApply = () => true,
                    Modifier = new Modifier { Operator = ModifierOperators.AddPercentage, Value = 10 }
                }
            };

            var service = new PricingService();
            var result = service.GetPrice(travel, rules);

            Assert.Equal(110, result);
        }

        [Fact]
        public void GetPrice_RemovePercentageModifier_ReturnsBasePriceMinusPercentage()
        {
            var travel = new Travel { BasePrice = 100 };
            var rules = new List<Rule>
            {
                new Rule
                {
                    ShouldApply = () => true,
                    Modifier = new Modifier { Operator = ModifierOperators.RemovePercentage, Value = 10 }
                }
            };

            var service = new PricingService();
            var result = service.GetPrice(travel, rules);

            Assert.Equal(90, result);
        }

        [Fact]
        public void GetPrice_FlatModifier_OverridesBasePrice()
        {
            var travel = new Travel { BasePrice = 100 };
            var rules = new List<Rule>
            {
                new Rule
                {
                    ShouldApply = () => true,
                    Modifier = new Modifier { Operator = ModifierOperators.Flat, Value = 25 }
                }
            };

            var service = new PricingService();
            var result = service.GetPrice(travel, rules);

            Assert.Equal(25, result);
        }

    }
}
