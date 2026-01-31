using MathTestSystem.Domain.Enums;
using System.Net.NetworkInformation;

namespace MathTestSystem.Domain.Entities
{
    public class TaskResult : BaseEntity
    {
        public Guid MathTaskId { get; set; }
        public decimal ExpectedResult { get; set; }
        public GradingStatus Status { get; set; }

        protected TaskResult() { }

        public TaskResult(Guid mathTaskId, decimal expectedResult, GradingStatus status)
        {
            MathTaskId = mathTaskId;
            ExpectedResult = expectedResult;
            Status = status;
        }
    }
}
