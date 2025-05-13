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
    public class RuleTests
    {
        [Fact]
        public void ShouldApply_StartLocationMatch_ReturnsTrue_WhenStartLocationMatches()
        {
            // Arrange
            var rule = new Rule
            {
                StartLocationMatch = "Jesi"
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
            // Arrange
            var rule = new Rule
            {
                StartLocationMatch = "Jesi"
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
            // Arrange
            var rule = new Rule
            {
                EndLocationMatch = "Senigallia"
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
            // Arrange
            var rule = new Rule
            {
                EndLocationMatch = "Senigallia"
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
            // Arrange
            var rule = new Rule
            {
                DayOfWeekMatch = DayOfWeek.Monday
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
            // Arrange
            var rule = new Rule
            {
                DayOfWeekMatch = DayOfWeek.Monday
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
            // Arrange
            var rule = new Rule
            {
                StartLocationMatch = "Jesi",
                EndLocationMatch = "Senigallia",
                DayOfWeekMatch = DayOfWeek.Monday
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
            // Arrange
            var rule = new Rule
            {
                StartLocationMatch = "Jesi",
                EndLocationMatch = "Senigallia",
                DayOfWeekMatch = DayOfWeek.Monday
            };

            // esempio con errata località di arrivo
            var travel = new Travel { StartLocation = "Jesi", EndLocation = "Ancona", Date = new DateTime(2025, 5, 12) }; 

            // Act
            var result = rule.ShouldApply(travel);

            // Assert
            Assert.False(result);
        }
    }


}
