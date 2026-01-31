using MathTestSystem.Domain.Interfaces;
using System.Text;

namespace MathTestSystem.Infrastructure.Helpers
{
    public class MathEvaluator : IMathEvaluator
    {
        public async Task<decimal> EvaluateAsync(string expression)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(expression))
                    throw new ArgumentException("Expression cannot be empty");

                expression = expression.Replace(" ", "");
                var tokens = Tokenize(expression);
                return EvaluateTokens(tokens);
            });
        }

        private List<string> Tokenize(string expr)
        {
            var tokens = new List<string>();
            var sb = new StringBuilder();

            foreach (var c in expr)
            {
                if ("+-*/".Contains(c))
                {
                    if (sb.Length > 0)
                    {
                        tokens.Add(sb.ToString());
                        sb.Clear();
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (sb.Length > 0) tokens.Add(sb.ToString());
            return tokens;
        }

        private decimal EvaluateTokens(List<string> tokens)
        {
            decimal result = decimal.Parse(tokens[0]);
            int i = 1;

            while (i < tokens.Count)
            {
                var op = tokens[i];
                var next = decimal.Parse(tokens[i + 1]);

                result = op switch
                {
                    "+" => result + next,
                    "-" => result - next,
                    "*" => result * next,
                    "/" => result / next,
                    _ => throw new InvalidOperationException($"Unknown operator: {op}")
                };

                i += 2;
            }

            return result;
        }
    }
}
