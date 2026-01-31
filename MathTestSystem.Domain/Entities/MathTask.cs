namespace MathTestSystem.Domain.Entities
{
    public class MathTask : BaseEntity
    {
        public string ExternalTaskId { get; private set; }
        public string Expression { get; private set; }
        public decimal SubmittedResult { get; private set; }

        protected MathTask() { }

        public MathTask(string externalTaskId, string expression, decimal submittedResult)
        {
            ExternalTaskId = externalTaskId;
            Expression = expression;
            SubmittedResult = submittedResult;
        }
    }
}
