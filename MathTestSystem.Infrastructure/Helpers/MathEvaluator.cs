using MathTestSystem.Domain.Interfaces;

public class MathEvaluator : IMathEvaluator
{
    private static readonly Dictionary<string, int> Precedence = new()
    {
        { "+", 1 },
        { "-", 1 },
        { "*", 2 },
        { "/", 2 },
        { "(", 0 }
    };

    public Task<decimal> EvaluateAsync(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentException("Expression is empty");

        var tokens = Tokenize(expression.Replace(" ", ""));
        var values = new Stack<decimal>();
        var ops = new Stack<string>();

        string? prevToken = null;

        foreach (var token in tokens)
        {

            if (decimal.TryParse(token, out var number))
            {
                values.Push(number);
            }

            else if (token == "(")
            {
                ops.Push(token);
            }

            else if (token == ")")
            {
                while (ops.Peek() != "(")
                    Apply(values, ops.Pop());

                ops.Pop();
            }

            else
            {

                if (token == "-" && (prevToken == null || prevToken == "(" || IsOperator(prevToken)))
                {
                    values.Push(0);
                }

                while (ops.Any() && Precedence[ops.Peek()] >= Precedence[token])
                {
                    Apply(values, ops.Pop());
                }

                ops.Push(token);
            }

            prevToken = token;
        }

        while (ops.Any())
            Apply(values, ops.Pop());

        return Task.FromResult(values.Pop());
    }

    private static void Apply(Stack<decimal> values, string op)
    {
        var b = values.Pop();
        var a = values.Pop();

        values.Push(op switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => a / b,
            _ => throw new InvalidOperationException($"Unknown operator {op}")
        });
    }

    private static bool IsOperator(string token)
        => token is "+" or "-" or "*" or "/";

    private static List<string> Tokenize(string expr)
    {
        var tokens = new List<string>();
        var current = "";

        foreach (var c in expr)
        {
            if ("+-*/()".Contains(c))
            {
                if (!string.IsNullOrEmpty(current))
                    tokens.Add(current);

                tokens.Add(c.ToString());
                current = "";
            }
            else
            {
                current += c;
            }
        }

        if (!string.IsNullOrEmpty(current))
            tokens.Add(current);

        return tokens;
    }
}