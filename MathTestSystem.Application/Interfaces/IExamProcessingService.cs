using MathTestSystem.Shared.DTOs;
using MathTestSystem.Domain.Entities;

namespace MathTestSystem.Shared.Interfaces
{
    public interface IExamProcessingService
    {
        Task<ExamResultDto> ProcessExamAsync(Exam exam);
    }
}
