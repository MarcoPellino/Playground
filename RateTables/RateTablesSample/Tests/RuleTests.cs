using RateTablesSample.Constants;
using RateTablesSample.Interfaces;
using RateTablesSample.Models;
using RateTablesSample.Models.Rules;
using RateTablesSample.Models.Rules.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RateTablesSample.Tests
{
    public class RuleTests
    {
        [Fact]
        public void ShouldApply_StartLocationMatch_ReturnsTrue_WhenStartLocationMatches()
        {
            var startFromJesi = new StartFromRuleSpecification("Jesi");
            // Arrange
            var rule = new Rule
            {
                ApplicationRule = startFromJesi
            };
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Senigallia", Date = DateTime.Now };

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldApply_StartLocationMatch_ReturnsFalse_WhenStartLocationDoesNotMatch()
        {
            var startFromJesi = new StartFromRuleSpecification("Jesi");
            // Arrange
            var rule = new Rule
            {
                ApplicationRule = startFromJesi
            };
            var travel = new Travel { StartLocation = "Ancona", EndLocation = "Senigallia", Date = DateTime.Now };

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShouldApply_EndLocationMatch_ReturnsTrue_WhenEndLocationMatches()
        {
            var endAtSenigallia = new EndAtRuleSpecification("Senigallia");
            // Arrange
            var rule = new Rule
            {
                ApplicationRule = endAtSenigallia
            };
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Senigallia", Date = DateTime.Now };

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldApply_EndLocationMatch_ReturnsFalse_WhenEndLocationDoesNotMatch()
        {
            var endAtSenigallia = new EndAtRuleSpecification("Senigallia");
            // Arrange
            var rule = new Rule
            {
                ApplicationRule = endAtSenigallia
            };
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Ancona", Date = DateTime.Now };

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShouldApply_DayOfWeekMatch_ReturnsTrue_WhenDayOfWeekMatches()
        {
            var travelOnMonday = new DayOfWeekRuleSpecification(DayOfWeek.Monday);
            // Arrange
            var rule = new Rule
            {
                ApplicationRule = travelOnMonday
            };
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Senigallia", Date = new DateTime(2025, 5, 12) }; // Monday

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldApply_DayOfWeekMatch_ReturnsFalse_WhenDayOfWeekDoesNotMatch()
        {
            var travelOnMonday = new DayOfWeekRuleSpecification(DayOfWeek.Monday);
            // Arrange
            var rule = new Rule
            {
                ApplicationRule = travelOnMonday
            };
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Senigallia", Date = new DateTime(2025, 5, 13) }; // Martedì

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShouldApply_CompositeConditions_ReturnsTrue_WhenAllConditionsMatch()
        {
            IRuleSpecification<Travel> travelRule = new StartFromRuleSpecification("Jesi");

            var ruleBuilder = new RuleBuilder<Travel>(travelRule);

            ruleBuilder = ruleBuilder.And(new EndAtRuleSpecification("Senigallia"));
            ruleBuilder = ruleBuilder.And(new DayOfWeekRuleSpecification(DayOfWeek.Monday));

            // Arrange
            var rule = new Rule
            {
                ApplicationRule = ruleBuilder.Build()
            };
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Senigallia", Date = new DateTime(2025, 5, 12) }; // Lunedì

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldApply_CompositeConditions_ReturnsFalse_WhenAnyConditionDoesNotMatch()
        {
            IRuleSpecification<Travel> startRule = new StartFromRuleSpecification("Jesi");

            var ruleBuilder = new RuleBuilder<Travel>(startRule);

            ruleBuilder = ruleBuilder.And(new EndAtRuleSpecification("Senigallia"));
            ruleBuilder = ruleBuilder.And(new DayOfWeekRuleSpecification(DayOfWeek.Monday));

            // Arrange
            var rule = new Rule
            {
                ApplicationRule = ruleBuilder.Build()
            };

            // esempio con errata località di arrivo
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Ancona", Date = new DateTime(2025, 5, 12) };

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShouldApply_OrCondition_ReturnsTrue_WhenAnyConditionMatches()
        {
            IRuleSpecification<Travel> travelRule = new StartFromRuleSpecification("Jesi");

            var ruleBuilder = new RuleBuilder<Travel>(travelRule);

            ruleBuilder = ruleBuilder.Or(new EndAtRuleSpecification("Senigallia"));
            ruleBuilder = ruleBuilder.Or(new DayOfWeekRuleSpecification(DayOfWeek.Monday));

            // Arrange
            var rule = new Rule
            {
                ApplicationRule = ruleBuilder.Build()
            };
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Senigallia", Date = new DateTime(2025, 5, 12) }; // Lunedì

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.True(result);  // La condizione "EndAt" o "DayOfWeek" deve essere soddisfatta
        }

        [Fact]
        public void ShouldApply_NotCondition_ReturnsFalse_WhenConditionDoesNotMatch()
        {
            IRuleSpecification<Travel> travelRule = new StartFromRuleSpecification("Jesi");

            var ruleBuilder = new RuleBuilder<Travel>(travelRule);

            ruleBuilder = ruleBuilder.And(new EndAtRuleSpecification("Senigallia"));
            ruleBuilder = ruleBuilder.And(new NotRuleSpecification<Travel>(new DayOfWeekRuleSpecification(DayOfWeek.Friday)));

            // Arrange
            var rule = new Rule
            {
                ApplicationRule = ruleBuilder.Build()
            };
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Senigallia", Date = new DateTime(2025, 5, 14) }; // Mercoledì (Non venerdì)

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.True(result);  // La condizione "Non Venerdì" deve essere soddisfatta, quindi il viaggio dovrebbe essere applicato
        }


    }


}
