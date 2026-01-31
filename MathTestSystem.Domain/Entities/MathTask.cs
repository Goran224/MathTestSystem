namespace MathTestSystem.Domain.Entities
{
    public class MathTask : BaseEntity
    {
        public string ExternalTaskId { get; set; } = string.Empty;
        public string Expression { get; set; } = string.Empty;
        public decimal SubmittedResult { get; set; }

        public Guid ExamId { get; set; }  // FK
        public Exam Exam { get; set; } = null!;

        public List<TaskResult> TaskResults { get; set; } = new();

        protected MathTask() { } // EF Core

        public MathTask(string externalTaskId, string expression, decimal submittedResult)
        {
            ExternalTaskId = externalTaskId;
            Expression = expression;
            SubmittedResult = submittedResult;
        }

        public void AddTaskResult(TaskResult result) => TaskResults.Add(result);
    }
}
