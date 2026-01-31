using MathTestSystem.Application.Contracts;
using MathTestSystem.Domain.Entities;

namespace MathTestSystem.Domain.Interfaces
{
    public interface IStudentParser
    {
        Task<List<Student>> ParseStudentsAsync(TeacherXml teacherXml);
    }
}
