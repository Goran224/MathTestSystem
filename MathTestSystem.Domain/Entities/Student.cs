namespace MathTestSystem.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string ExternalStudentId { get; set; } = string.Empty;

        // EF Core friendly
        public List<Exam> Exams { get; set; } = new();

        protected Student() { } // EF Core

        public Student(string externalStudentId)
        {
            ExternalStudentId = externalStudentId;
        }

        public void AddExam(Exam exam) => Exams.Add(exam);
    }
}
