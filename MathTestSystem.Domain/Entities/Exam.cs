namespace MathTestSystem.Domain.Entities
{
    public class Exam : BaseEntity
    {
        public string ExternalExamId { get; set; } = string.Empty;

        // Simplified: use a public list so EF Core can map it
        public List<MathTask> Tasks { get; set; } = new();

        protected Exam() { } // EF Core

        public Exam(string externalExamId)
        {
            ExternalExamId = externalExamId;
        }

        public void AddTask(MathTask task) => Tasks.Add(task);
    }
}
