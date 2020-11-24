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

        [Test]
        public void Parse_ShouldReturnFour_WhenArgsEqualSevenMinusThree()
        {
            var calc = new Calculate();
            var result =  calc.Parse(new Expression("7-3", calc)).Result;
            result.Should().Be(4.0).And.BePositive("Some problem");
        }
        [Test]
        public void Parse_ShouldReturnSix_WhenArgsEqualTwoMultiplyThree()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("2*3/1", calc)).Result;
            result.Should().Be(6.0).And.BePositive("Some problem");
        }
        [Test]
        public void Parse_ShouldReturnTen_WhenArgsEqualFiveSumFive()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("5+5", calc)).Result;
            result.Should().Be(10.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnNine_WhenArgsEqualOnePlusTwoMultiplyThreeDivisionOne()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("1+2*3/1", calc)).Result;
            result.Should().Be(7.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFivePlus()
        {
            var calc = new Calculate();
            var exp = calc.Parse(new Expression("5+", calc));
            var result = exp.Result;
            result.Should().Be(0.0);
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFiveMinus()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("5-", calc)).Result;
            result.Should().Be(0.0);
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFiveMultiply()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("5*", calc)).Result;
            result.Should().Be(0.0);
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFiveDivision()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("5/", calc)).Result;
            result.Should().Be(0.0);
        }

        [Test]
        public void Parse_ShouldReturnMinusTwo_WhenArgsEqualMinusTwo()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("-1+2-3-4", calc)).Result;
            result.Should().Be(-6.0).And.BeNegative("Some problem");
        }
        [Test]
        public void Parse_ShouldReturnMinusOne_WhenArgsEqualMinusFivePlusSix()
        {
            var calc = new Calculate();
            var result = calc.Parse(new Expression("-5+6", calc)).Result;
            result.Should().Be(1.0);
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
        public void Parse_ShouldReturnNinetySix_WhenArgsEqualTwoMultiplyTwoPlusTwoInBracket()
        {
            var calc = new Calculate();
            var expression = calc.Parse(new Expression("2*((2+2)*3+4)", calc));
            var result = expression.Result;
            result.Should().Be(32.0);
        }

        [Test]
        public void Parse_ShouldReturnError_WhenArgsTwoMultiplyCloseBracketTwoPlusTwoOpenBracket()
        {
            var calc = new Calculate();
            var expression = calc.Parse(new Expression("2*)2+2(", calc));
            var result = expression.HasError;
            result.Should().Be(true);
        }

        [Test]
        public void Parse_Test2()
        {
            var calc = new Calculate();
            var expression = calc.Parse(new Expression("10-2+10", calc));
            var result = expression.Result;
            result.Should().Be(18.0);
        }
    }
}