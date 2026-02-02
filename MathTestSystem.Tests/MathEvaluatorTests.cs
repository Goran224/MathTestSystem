using MathTestSystem.Infrastructure.Helpers;

namespace MathTestSystem.Tests;

public class MathEvaluatorTests
{
    private readonly MathEvaluator _evaluator = new();

    [Theory]
    [InlineData("2+2", 4)]
    [InlineData("2+3*4", 14)]
    [InlineData("(2+3)*4", 20)]
    [InlineData("10/4", 2.5)]
    public async Task EvaluateAsync_ReturnsExpectedResult(string expression, decimal expected)
    {
        var result = await _evaluator.EvaluateAsync(expression);

        Assert.Equal(expected, result);
    }
}