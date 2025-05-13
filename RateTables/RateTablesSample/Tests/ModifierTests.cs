using RateTablesSample.Constants;
using RateTablesSample.Models;
using Xunit;

namespace RateTablesSample.Tests
{
    public class ModifierTests
    {
        [Fact]
        public void Apply_AddOperator_ReturnsPricePlusValue()
        {
            var modifier = new Modifier { Operator = ModifierOperators.Add, Value = 10 };
            var result = modifier.Apply(100);
            Assert.Equal(110, result);
        }

        [Fact]
        public void Apply_SubstractOperator_ReturnsPriceMinusValue()
        {
            var modifier = new Modifier { Operator = ModifierOperators.Substract, Value = 10 };
            var result = modifier.Apply(100);
            Assert.Equal(90, result);
        }

        [Fact]
        public void Apply_MultiplyOperator_ReturnsPriceTimesValue()
        {
            var modifier = new Modifier { Operator = ModifierOperators.Multiply, Value = 2 };
            var result = modifier.Apply(100);
            Assert.Equal(200, result);
        }

        [Fact]
        public void Apply_DivideOperator_ReturnsPriceDividedByValue()
        {
            var modifier = new Modifier { Operator = ModifierOperators.Divide, Value = 2 };
            var result = modifier.Apply(100);
            Assert.Equal(50, result);
        }

        [Fact]
        public void Apply_AddPercentageOperator_ReturnsPriceIncreasedByPercentage()
        {
            var modifier = new Modifier { Operator = ModifierOperators.AddPercentage, Value = 10 };
            var result = modifier.Apply(100);
            Assert.Equal(110, result); // 100 + 10%
        }

        [Fact]
        public void Apply_RemovePercentageOperator_ReturnsPriceReducedByPercentage()
        {
            var modifier = new Modifier { Operator = ModifierOperators.RemovePercentage, Value = 10 };
            var result = modifier.Apply(100);
            Assert.Equal(90, result); // 100 - 10%
        }

        [Fact]
        public void Apply_FlatOperator_ReturnsFlatValue()
        {
            var modifier = new Modifier { Operator = ModifierOperators.Flat, Value = 42 };
            var result = modifier.Apply(100);
            Assert.Equal(42, result);
        }

        [Fact]
        public void Apply_UnknownOperator_ReturnsOriginalPrice()
        {
            var modifier = new Modifier { Operator = (ModifierOperators)(-1), Value = 999 };
            var result = modifier.Apply(100);
            Assert.Equal(100, result); // fallback default
        }
    }

}
