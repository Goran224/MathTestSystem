using MathTestSystem.Shared.Contracts;
using MathTestSystem.Domain.Entities;
using MathTestSystem.Infrastructure.Helpers;
using MathTestSystem.Shared.Interfaces;

public class StudentParser : IStudentParser
{
    public Task<List<Student>> ParseStudentsAsync(TeacherXml teacherXml)
    {
        if (teacherXml?.Students == null)
            return Task.FromResult(new List<Student>());

        var students = new List<Student>();

        foreach (var studentXml in teacherXml.Students)
        {
            var student = XmlMapper.MapToStudent(studentXml);
            students.Add(student);
        }

        return Task.FromResult(students);
    }
}
