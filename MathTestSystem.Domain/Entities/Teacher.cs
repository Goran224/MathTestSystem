namespace MathTestSystem.Domain.Entities
{
    public class Teacher : BaseEntity
    {
        public string ExternalTeacherId { get; set; } = string.Empty;

        public List<Student> Students { get; set; } = new();

        protected Teacher() { }

        public Teacher(string externalTeacherId)
        {
            ExternalTeacherId = externalTeacherId;
        }
    }
}
