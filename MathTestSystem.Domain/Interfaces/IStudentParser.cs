using MathTestSystem.Domain.Entities;

namespace MathTestSystem.Domain.Interfaces
{
    public interface IStudentParser
    {
        Task<List<Student>> ParseStudentsAsync(string xmlContent);
    }
}
