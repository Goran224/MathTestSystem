using MathTestSystem.Domain.Enums;

namespace MathTestSystem.Application.DTOs
{
    public class TaskResultDto
    {
        public Guid MathTaskId { get; set; }
        public decimal ExpectedResult { get; set; }
        public GradingStatus Status { get; set; }
    }
}
