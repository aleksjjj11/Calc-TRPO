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
            var result =  calc.Parse("7-3").Result;
            result.Should().Be(4.0).And.BePositive("Some problem");
        }
        [Test]
        public void Parse_ShouldReturnSix_WhenArgsEqualTwoMultiplyThree()
        {
            var calc = new Calculate();
            var result = calc.Parse("2*3/1").Result;
            result.Should().Be(6.0).And.BePositive("Some problem");
        }
        [Test]
        public void Parse_ShouldReturnTen_WhenArgsEqualFiveSumFive()
        {
            var calc = new Calculate();
            var result = calc.Parse("5+5").Result;
            result.Should().Be(10.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnNine_WhenArgsEqualOnePlusTwoMultiplyThreeDivisionOne()
        {
            var calc = new Calculate();
            var result = calc.Parse("1+2*3/1").Result;
            result.Should().Be(7.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFivePlus()
        {
            var calc = new Calculate();
            var result = calc.Parse("5+").Result;
            result.Should().Be(5.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFiveMinus()
        {
            var calc = new Calculate();
            var result = calc.Parse("5-").Result;
            result.Should().Be(5.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFiveMultiply()
        {
            var calc = new Calculate();
            var result = calc.Parse("5*").Result;
            result.Should().Be(5.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFiveDivision()
        {
            var calc = new Calculate();
            var result = calc.Parse("5/").Result;
            result.Should().Be(5.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnMinusTwo_WhenArgsEqualMinusTwo()
        {
            var calc = new Calculate();
            var result = calc.Parse("-1+2-3-4").Result;
            result.Should().Be(-6.0).And.BeNegative("Some problem");
        }
    }
}