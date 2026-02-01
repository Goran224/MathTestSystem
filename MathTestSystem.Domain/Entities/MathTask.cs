using MathTestSystem.Domain.Enums;

namespace MathTestSystem.Domain.Entities
{
    public class MathTask : BaseEntity
    {
        public string ExternalTaskId { get; set; } = string.Empty;
        public string Expression { get; set; } = string.Empty;
        public decimal SubmittedResult { get; set; }

        public decimal ExpectedResult { get; set; }    // new
        public GradingStatus Status { get; set; }      // new

        public Guid ExamId { get; set; }
        public Exam Exam { get; set; } = null!;

        protected MathTask() { }

        public MathTask(string externalTaskId, string expression, decimal submittedResult)
        {
            ExternalTaskId = externalTaskId;
            Expression = expression;
            SubmittedResult = submittedResult;
        }
    }
}
