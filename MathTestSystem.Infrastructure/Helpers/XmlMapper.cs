namespace MathTestSystem.Infrastructure.Helpers
{
    using MathTestSystem.Shared.Contracts;
    using MathTestSystem.Domain.Entities;

    public static class XmlMapper
    {
        public static Student MapToStudent(StudentXml studentXml)
        {
            var student = new Student(studentXml.ID.ToString());

            if (studentXml.Exams != null)
            {
                foreach (var examXml in studentXml.Exams)
                {
                    var exam = MapToExam(examXml);
                    student.AddExam(exam);
                }
            }

            return student;
        }

        public static Exam MapToExam(ExamXml examXml)
        {
            var exam = new Exam(examXml.Id.ToString());

            if (examXml.Tasks != null)
            {
                foreach (var taskXml in examXml.Tasks)
                {
                    var task = MapToTask(taskXml);
                    exam.AddTask(task);
                }
            }

            return exam;
        }

        public static MathTask MapToTask(TaskXml taskXml)
        {
            var parts = taskXml.Value.Split('=');
            if (parts.Length != 2)
                return new MathTask(taskXml.Id.ToString(), taskXml.Value, 0);

            var expression = parts[0].Trim();
            var submitted = decimal.TryParse(parts[1].Trim(), out var r) ? r : 0;

            return new MathTask(taskXml.Id.ToString(), expression, submitted);
        }
    }

}
