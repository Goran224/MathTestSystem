namespace MathTestSystem.Domain.Entities
{
    public class Exam : BaseEntity
    {
        public string ExternalExamId { get; set; } = string.Empty;

        // EF Core friendly
        public List<MathTask> Tasks { get; set; } = new();

        public Guid StudentId { get; set; }  // FK
        public Student Student { get; set; } = null!;

        protected Exam() { } // EF Core

        public Exam(string externalExamId)
        {
            ExternalExamId = externalExamId;
        }

        public void AddTask(MathTask task) => Tasks.Add(task);
    }
}
