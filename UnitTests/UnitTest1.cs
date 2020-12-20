using System.Diagnostics.CodeAnalysis;
using Calc.Models;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(7, 3, '-', 4)]
        [TestCase(5, 5, '+', 10)]
        [TestCase(-5, 6, '+', 1)]
        public void Parse_ShouldReturnFResult_WhenArgsEqualSVal1OperationVal2(double val1, double val2, char operation, double res)
        {
            var calc = new Calculate();
            var result =  calc.Parse(new Expression($"{val1}{operation}{val2}", calc)).Result;
            result.Should().Be(res);
        }
        [Test]
        public void Parse_ShouldReturnSix_WhenArgsEqualTwoMultiplyThree()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("2*3/1", calc)).Result;
            result.Should().Be(6.0);
        }

        [Test]
        public void Parse_ShouldReturnSeven_WhenArgsEqualOnePlusTwoMultiplyThreeDivisionOne()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("1+2*3/1", calc)).Result;
            result.Should().Be(7.0);
        }

        [TestCase(5, '+', true)]
        [TestCase(5, '-', true)]
        [TestCase(5, '*', true)]
        [TestCase(5, '/', true)]
        public void Parse_ShouldReturnHasErrorTrue(double val, char operation, bool res)
        {
            var calc = new Calculate();
            var exp = calc.Parse(new Expression($"{val}{operation}", calc));
            var result = exp.HasError;
            result.Should().Be(res);
        }

        [Test]
        public void Parse_ShouldReturnMinusSix_WhenArgsEqualMinusOnePlusTwoMinusThreeMinusFour()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("-1+2-3-4", calc)).Result;
            result.Should().Be(-6.0);
        }

        [Test]
        public void Parse_ShouldReturnEight_WhenArgsEqualTwoMultiplyTwoPlusTwoInBracket()
        {
            var calc = new Calculate();
            var expression = calc.Parse(new Expression("2*(2+2)", calc));
            var result = expression.Result;
            result.Should().Be(8.0);
        }
        [Test]
        public void Parse_ShouldReturnThirtyTwo_WhenArgsEqualTwoMultiplyTwoPlusTwoInBracketMultiplyThreePlusFourInBracket()
        {
            var calc = new Calculate();
            var expression = calc.Parse(new Expression("2*((2+2)*3+4)", calc));
            var result = expression.Result;
            result.Should().Be(32.0);
        }

        [Test]
        public void Parse_ShouldReturnHasErrorTrue_WhenArgsEqualTwoMultiplyCloseBracketTwoPlusTwoOpenBracket()
        {
            var calc = new Calculate();
            var expression = calc.Parse(new Expression("2*)2+2(", calc));
            var result = expression.HasError;
            result.Should().Be(true);
        }

        [Test]
        public void Parse_ShouldReturnEighteen_WhenArgsEqualTenMinusTwoPlusTen()
        {
            var calc = new Calculate();
            var expression = calc.Parse(new Expression("10-2+10", calc));
            var result = expression.Result;
            result.Should().Be(18.0);
        }
    }
}