namespace MathTestSystem.Domain.Interfaces
{
    public interface IMathEvaluator
    {
        Task<decimal> EvaluateAsync(string expression);
    }
}
