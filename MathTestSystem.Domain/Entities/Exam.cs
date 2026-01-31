namespace MathTestSystem.Domain.Entities
{
    public class Exam : BaseEntity
    {
        public string ExternalExamId { get; private set; }

        private readonly List<MathTask> _tasks = new();
        public IReadOnlyCollection<MathTask> Tasks => _tasks;

        protected Exam() { }

        public Exam(string externalExamId)
        {
            ExternalExamId = externalExamId;
        }

        public void AddTask(MathTask task)
        {
            _tasks.Add(task);
        }
    }
}
