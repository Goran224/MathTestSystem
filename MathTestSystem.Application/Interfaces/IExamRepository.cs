using MathTestSystem.Shared.DTOs;
using MathTestSystem.Domain.Entities;

namespace MathTestSystem.Shared.Interfaces
{
    public interface IExamRepository
    {
        Task SaveTeacherTreeAsync(Teacher teacher);

        Task<List<ExamDto>> GetAllExamsAsync();
        Task<List<ExamDto>> GetExamsByStudentAsync(string externalStudentId);
        Task<List<ExamDto>> GetExamsByTeacherAsync(string externalTeacherId);

        Task UpdateExamAsync(Exam exam);
    }
}
