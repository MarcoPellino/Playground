using RateTablesSample.BOL;
using RateTablesSample.Constants;
using RateTablesSample.Interfaces;
using RateTablesSample.Models;
using RateTablesSample.Models.Rules.Base;
using RateTablesSample.Models.Rules;
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
                    ApplicationRule = new TrueRuleSpecification(),
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
                    ApplicationRule = new TrueRuleSpecification(),
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
                    ApplicationRule = new TrueRuleSpecification(),
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
                    ApplicationRule = new TrueRuleSpecification(),
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
                    ApplicationRule = new TrueRuleSpecification(),
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
                    ApplicationRule = new TrueRuleSpecification(),
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
                {ApplicationRule = new TrueRuleSpecification(),
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
                    ApplicationRule = new TrueRuleSpecification(),
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Add,
                        Value = 10,
                        StopApplyOthers = false
                    }
                },
                new Rule
                {
                    ApplicationRule = new TrueRuleSpecification(),
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Multiply,
                        Value = 2,
                        StopApplyOthers = false
                    }
                },
                new Rule
                {
                    ApplicationRule = new TrueRuleSpecification(),
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
                    ApplicationRule = new TrueRuleSpecification(),
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Add,
                        Value = 10,
                        StopApplyOthers = false
                    }
                },
                new Rule
                {
                    ApplicationRule = new TrueRuleSpecification(),
                    Modifier = new Modifier
                    {
                        Operator = ModifierOperators.Flat,
                        Value = 50,
                        StopApplyOthers = true
                    }
                },
                new Rule
                {
                    ApplicationRule = new TrueRuleSpecification(),
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

            // Rule 1: tutti i viaggi che partono da Jesi, Ancona, Senigallia, Arcevia
            var startRule = new StartFromRuleSpecification("Jesi");
            var ruleBuilder = new RuleBuilder<Travel>(startRule);
            ruleBuilder = ruleBuilder.Or(new StartFromRuleSpecification("Ancona"));
            ruleBuilder = ruleBuilder.Or(new StartFromRuleSpecification("Senigallia"));
            ruleBuilder = ruleBuilder.Or(new StartFromRuleSpecification("Arcevia"));

            // Rule 2: tutti i viaggi che partono da Jesi e finiscono ad Ancona, Roma, Firenze -> da non applicare
            var startRuleEndRule = new StartFromRuleSpecification("Jesi");
            var ruleBuilderEndRule = new RuleBuilder<Travel>(startRuleEndRule).And(
                    new RuleBuilder<Travel>(new EndAtRuleSpecification("Ancona"))
                        .Or(new EndAtRuleSpecification("Roma"))
                        .Or(new EndAtRuleSpecification("Firenze"))
                        .Build()
                );

            // Rule 3: tutti i viaggi del mercoledì -> da  applicare
            var dayRule = new DayOfWeekRuleSpecification(DayOfWeek.Wednesday);

            // Rule 4: tutti i viaggi che partono da Senigallia il venerdì -> non applicare
            var senigalliaRuleFriday = new StartFromRuleSpecification("Senigallia");
            var ruleBuilderSenigalliaFriday = new RuleBuilder<Travel>(senigalliaRuleFriday).And(new DayOfWeekRuleSpecification(DayOfWeek.Friday));

            // Rule 4: tutti i viaggi che partono da Jesi e arrivano in Ancona, nei giorni Lunedì, Martedì, Mercoledì
            var anconaRule = new StartFromRuleSpecification("Jesi");
            var ruleBuilderAncona = new RuleBuilder<Travel>(anconaRule)
                .And(new EndAtRuleSpecification("Ancona"))
                .And(
                    new RuleBuilder<Travel>(new DayOfWeekRuleSpecification(DayOfWeek.Monday))
                        .Or(new DayOfWeekRuleSpecification(DayOfWeek.Tuesday))
                        .Or(new DayOfWeekRuleSpecification(DayOfWeek.Wednesday))
                        .Build()
            );

            // Rule 5: tutti i viaggi che finiscono in Ancona, Senigallia o Jesi
            var endRule = new EndAtRuleSpecification("Ancona");
            var endRuleAnconaJesiSenBuild = new RuleBuilder<Travel>(endRule)
                .Or(new EndAtRuleSpecification("Senigallia"))
                .Or(new EndAtRuleSpecification("Jesi"));

            var rules = new List<Rule>
            {
            new Rule
            {
                ApplicationRule = ruleBuilder.Build(),
                Modifier = new Modifier
                {
                    Operator = ModifierOperators.Add,
                    Value = 10,
                    StopApplyOthers = false
                }
            },

            // Rule 2: non si applica
            new Rule
            {
                ApplicationRule = ruleBuilderEndRule.Build(),
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
                ApplicationRule = dayRule,
                Modifier = new Modifier
                {
                    Operator = ModifierOperators.Substract,
                    Value = 5,
                    StopApplyOthers = false
                }
            },

            // Rule 4: match inizio viaggio e giorno settimana ma non arrivo - should NOT apply
            new Rule
            {
                ApplicationRule = ruleBuilderAncona.Build(),
                Modifier = new Modifier
                {
                    Operator = ModifierOperators.Flat,
                    Value = 1,
                    StopApplyOthers = false
                }
            },

            // Rule 5: match DayOfWeek only - should apply
            new Rule
            {
                ApplicationRule = endRuleAnconaJesiSenBuild.Build(),
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
            // Rule 5: *2 → 210
            Assert.Equal(210, result);
        }

    }
}
