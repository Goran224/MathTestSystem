namespace MathTestSystem.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string ExternalStudentId { get; set; } = string.Empty;

        // Simplified: use a public list
        public List<Exam> Exams { get; set; } = new();

        protected Student() { }

        public Student(string externalStudentId)
        {
            ExternalStudentId = externalStudentId;
        }

        public void AddExam(Exam exam) => Exams.Add(exam);
    }
}
