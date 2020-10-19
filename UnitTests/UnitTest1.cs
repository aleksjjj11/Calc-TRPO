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
            var result = Calculate.Parse("7-3");
            result.Should().Be(4.0).And.BePositive("Some problem");
        }
        [Test]
        public void Parse_ShouldReturnSix_WhenArgsEqualTwoMultiplyThree()
        {
            var result = Calculate.Parse("2*3");
            result.Should().Be(6.0).And.BePositive("Some problem");
        }
        [Test]
        public void Parse_ShouldReturnTen_WhenArgsEqualFiveSumFive()
        {
            var result = Calculate.Parse("5+5");
            result.Should().Be(10.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnNine_WhenArgsEqualOnePlusTwoMultiplyThreeDivisionOne()
        {
            var result = Calculate.Parse("1+2*3/1");
            result.Should().Be(9.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFivePlus()
        {
            var result = Calculate.Parse("5+");
            result.Should().Be(5.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFiveMinus()
        {
            var result = Calculate.Parse("5-");
            result.Should().Be(5.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFiveMultiply()
        {
            var result = Calculate.Parse("5*");
            result.Should().Be(5.0).And.BePositive("Some problem");
        }

        [Test]
        public void Parse_ShouldReturnFive_WhenArgsEqualFiveDivision()
        {
            var result = Calculate.Parse("5/");
            result.Should().Be(5.0).And.BePositive("Some problem");
        }
    }
}