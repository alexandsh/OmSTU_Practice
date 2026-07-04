namespace task11tests;

using Xunit;
using task11;
public class CalculatorTests
{
    private readonly ICalculator calculator = GenerateInRunTime.GenerateCalculator();

    [Fact]
    public void TestAdd()
    {
        Assert.Equal(5, calculator.Add(3, 2));
    }

    [Fact]
    public void TestMinus()
    {
        Assert.Equal(12, calculator.Minus(24, 12));
    }

    [Fact]
    public void TestMul()
    {
        Assert.Equal(15, calculator.Mul(5, 3));
    }

    [Fact]
    public void TestDiv()
    {
        Assert.Equal(2, calculator.Div(10, 5));
    }
}