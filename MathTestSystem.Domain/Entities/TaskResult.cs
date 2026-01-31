using MathTestSystem.Domain.Enums;
using System.Net.NetworkInformation;

namespace MathTestSystem.Domain.Entities
{
    public class TaskResult : BaseEntity
    {
        public Guid MathTaskId { get; set; }
        public MathTask MathTask { get; set; } = null!;

        public decimal ExpectedResult { get; set; }
        public decimal SubmittedResult { get; set; }  // now persisted
        public GradingStatus Status { get; set; }

        protected TaskResult() { } // EF Core

        public TaskResult(Guid mathTaskId, decimal expectedResult, decimal submittedResult, GradingStatus status)
        {
            MathTaskId = mathTaskId;
            ExpectedResult = expectedResult;
            SubmittedResult = submittedResult;
            Status = status;
        }
    }
}
