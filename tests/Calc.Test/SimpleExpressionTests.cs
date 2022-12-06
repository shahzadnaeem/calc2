using CalcLib;
using Sprache;
using Xunit;

namespace Calc.Test;

public class SimpleExpressionTests
{
    const double EPSILON = 1e-10;

    [Theory]
    [InlineData("1+1", 2)]
    [InlineData(" 1+1", 2)]
    [InlineData(" 1 + 1 ", 2)]
    [InlineData("1+1 ", 2)]
    public void BinaryOperations(string expr, double expected)
    {
        var result = ExpressionParser.ParseExpression(expr).Compile()();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("2^3", 8)]
    [InlineData("8^(1/3)", 2)]
    public void PowerOperations(string expr, double expected)
    {
        var result = ExpressionParser.ParseExpression(expr).Compile()();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("-1", -1)]
    [InlineData("0", 0)]
    [InlineData("-0", 0)]
    [InlineData("PI", Math.PI)]
    public void Constants(string expr, double expected)
    {
        var result = ExpressionParser.ParseExpression(expr).Compile()();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Sin(PI)", 0)]
    [InlineData("Sin(PI/2)", 1.0)]
    [InlineData("Sin(PI/6)", 0.5)]
    [InlineData("Cos(Tau)", 1.0)]
    [InlineData("Sign(Tau)", 1.0)]
    public void Functions(string expr, double expected)
    {
        var result = ExpressionParser.ParseExpression(expr).Compile()();

        Assert.Equal(expected, result, EPSILON);
    }

    [Theory]
    [InlineData("1+")]
    [InlineData("+1")]
    [InlineData("2*(3+")]
    [InlineData("NOTACONSTANT")]
    [InlineData("NotAFunction(0)")]
    [InlineData("Sin(BADCONST)")]
    public void ParseExceptions(string expr)
    {
        Assert.Throws<ParseException>(() => ExpressionParser.ParseExpression(expr));
    }
}
