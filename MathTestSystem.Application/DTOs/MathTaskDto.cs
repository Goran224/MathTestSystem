using MathTestSystem.Domain.Enums;

namespace MathTestSystem.Shared.DTOs
{
    public class MathTaskDto
    {
        public string Expression { get; set; } = string.Empty;
        public decimal SubmittedResult { get; set; }
        public decimal ExpectedResult { get; set; }
        public GradingStatus Status { get; set; }
    }
}
