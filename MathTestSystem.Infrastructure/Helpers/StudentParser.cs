using MathTestSystem.Application.Contracts;
using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Interfaces;

public class StudentParser : IStudentParser
{
    public Task<List<Student>> ParseStudentsAsync(TeacherXml teacherXml)
    {
        if (teacherXml?.Students == null)
            return Task.FromResult(new List<Student>());

        var students = new List<Student>();

        foreach (var studentXml in teacherXml.Students) // ✅ direct iteration
        {
            var student = XmlMapper.MapToStudent(studentXml);
            students.Add(student);
        }

        return Task.FromResult(students);
    }
}

// XML Mapper helper
public static class XmlMapper
{
    public static Student MapToStudent(StudentXml studentXml)
    {
        var student = new Student(studentXml.ID.ToString()); // assuming domain wants string

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
        // parse expression and submitted result
        var parts = taskXml.Value.Split('=');
        if (parts.Length != 2)
            return new MathTask(taskXml.Id.ToString(), taskXml.Value, 0); // fallback

        var expression = parts[0].Trim();
        var submitted = decimal.TryParse(parts[1].Trim(), out var r) ? r : 0;

        return new MathTask(taskXml.Id.ToString(), expression, submitted);
    }
}