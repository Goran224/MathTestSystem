using MathTestSystem.Domain.Enums;

namespace MathTestSystem.Application.DTOs
{
    public class TaskResultDto
    {
        public Guid MathTaskId { get; set; }
        public string Expression { get; set; } = string.Empty;
        public decimal SubmittedResult { get; set; }
        public decimal ExpectedResult { get; set; }
        public GradingStatus Status { get; set; }
    }
}
