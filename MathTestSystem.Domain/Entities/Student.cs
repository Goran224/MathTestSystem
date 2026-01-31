namespace MathTestSystem.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string ExternalStudentId { get; private set; }

        private readonly List<Exam> _exams = new();
        public IReadOnlyCollection<Exam> Exams => _exams;

        protected Student() { }

        public Student(string externalStudentId)
        {
            ExternalStudentId = externalStudentId;
        }

        public void AddExam(Exam exam)
        {
            _exams.Add(exam);
        }
    }
}
