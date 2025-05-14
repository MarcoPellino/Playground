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
            var travel = new Travel { BasePrice = 50 };
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
                    Modifier = new Modifier { Operator = ModifierOperators.Flat, Value = 25 }
                }
            };

            var service = new PricingService();
            var result = service.GetPrice(travel, rules);

            Assert.Equal(25, result);
        }


        [Fact]
        public void GetPrice_ThreeSequentialModifiers_AllAppliedInOrder()
        {
            // Arrange
            var travel = new Travel { BasePrice = 100 };

            // Prezzo base : 100
            // prima regola + 10 = 110
            // seconda regola * 2 = 220
            // terza regola - 20 = 200

            var rules = new List<Rule>
            {
                new Rule
                {
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Add,
                        Value = 10,
                        StopApplyOthers = false
                    }
                },
                new Rule
                {
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Multiply,
                        Value = 2,
                        StopApplyOthers = false
                    }
                },
                new Rule
                {
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Substract,
                        Value = 20,
                        StopApplyOthers = false
                    }
                }
            };

            var service = new PricingService();

            // Act
            var result = service.GetPrice(travel, rules);

            // Assert
            Assert.Equal(200, result);
        }

        [Fact]
        public void GetPrice_FlatModifierStopsFurtherRules_OnlyUpToFlatApplied()
        {
            // Arrange
            var travel = new Travel { BasePrice = 100 };
            var rules = new List<Rule>
            {
                new Rule
                {
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Add,
                        Value = 10,
                        StopApplyOthers = false
                    }
                },
                new Rule
                {
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Flat,
                        Value = 50,
                        StopApplyOthers = true
                    }
                },
                new Rule
                {
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Multiply,
                        Value = 2,
                        StopApplyOthers = false
                    }
                }
            };

            var service = new PricingService();

            // Act
            var result = service.GetPrice(travel, rules);

            // Assert
            Assert.Equal(50, result);
        }

        [Fact]
        public void GetPrice_ComplexScenario_WithSelectiveRuleApplication()
        {
            // Arrange
            var travelDate = new DateTime(2025, 5, 14); // Mercoledì
            var travel = new Travel
            {
                StartLocation = "Jesi",
                EndLocation = "Senigallia",
                Date = travelDate,
                BasePrice = 100
            };

            var rules = new List<Rule>
        {
        // Rule 1: match StartLocation only - should apply
        new Rule
        {
            StartLocationMatch = "Jesi",
            Modifier = new Modifier
            {
                Operator = ModifierOperators.Add,
                Value = 10,
                StopApplyOthers = false
            }
        },

        // Rule 2: unrelated - should not apply
        new Rule
        {
            StartLocationMatch = "Firenze",
            Modifier = new Modifier
            {
                Operator = ModifierOperators.Add,
                Value = 999,
                StopApplyOthers = false
            }
            },

        // Rule 3: match EndLocation and DayOfWeek - should apply
        new Rule
        {
            EndLocationMatch = "Senigallia",
            DayOfWeekMatch = DayOfWeek.Wednesday,
            Modifier = new Modifier
            {
                Operator = ModifierOperators.Substract,
                Value = 5,
                StopApplyOthers = false
            }
        },

        // Rule 4: match nothing - should NOT apply
        new Rule
        {
            StartLocationMatch = "Senigallia",
            EndLocationMatch = "Roma",
            DayOfWeekMatch = DayOfWeek.Friday,
            Modifier = new Modifier
            {
                Operator = ModifierOperators.Flat,
                Value = 1,
                StopApplyOthers = false
            }
        },

        // Rule 5: only StartLocation match fails - should NOT apply
        new Rule
        {
            StartLocationMatch = "Ancona",
            EndLocationMatch = "Senigallia",
            Modifier = new Modifier
            {
                Operator = ModifierOperators.AddPercentage,
                Value = 50,
                StopApplyOthers = true
            }
        },

        // Rule 6: only DayOfWeek mismatch - should NOT apply
        new Rule
        {
            StartLocationMatch = "Jesi",
            EndLocationMatch = "Senigallia",
            DayOfWeekMatch = DayOfWeek.Sunday,
            Modifier = new Modifier
            {
                Operator = ModifierOperators.RemovePercentage,
                Value = 10,
                StopApplyOthers = false
            }
        },

        // Rule 7: match DayOfWeek only - should apply
        new Rule
        {
            DayOfWeekMatch = DayOfWeek.Wednesday,
            Modifier = new Modifier
            {
                Operator = ModifierOperators.Multiply,
                Value = 2,
                StopApplyOthers = false
            }
        }
        };

            var service = new PricingService();

            // Act
            var result = service.GetPrice(travel, rules);

            // Assert
            // BasePrice: 100
            // Rule 1: +10 → 110
            // Rule 3: -5 → 105
            // Rule 7: *2 → 210
            Assert.Equal(210, result);
        }

    }
}
