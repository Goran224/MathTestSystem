using MathTestSystem.Shared.Contracts;
using MathTestSystem.Domain.Entities;

namespace MathTestSystem.Shared.Interfaces
{
    public interface IStudentParser
    {
        Task<List<Student>> ParseStudentsAsync(TeacherXml teacherXml);
    }
}
