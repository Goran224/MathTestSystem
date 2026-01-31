namespace MathTestSystem.Domain.Entities
{
    public class TaskResult : BaseEntity
    {
        public Guid MathTaskId { get; private set; }
        public decimal ExpectedResult { get; private set; }
        public TaskStatus Status { get; private set; }

        protected TaskResult() { }

        public TaskResult(Guid mathTaskId, decimal expectedResult, TaskStatus status)
        {
            MathTaskId = mathTaskId;
            ExpectedResult = expectedResult;
            Status = status;
        }
    }
}
