namespace MathTestSystem.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string ExternalStudentId { get; set; } = string.Empty;

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
        public List<Exam> Exams { get; set; } = new();

        protected Student() { } 

        public Student(string externalStudentId)
        {
            ExternalStudentId = externalStudentId;
        }

        public void AddExam(Exam exam) => Exams.Add(exam);
    }
}
