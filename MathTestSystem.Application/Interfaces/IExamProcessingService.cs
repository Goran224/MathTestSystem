using MathTestSystem.Application.DTOs;
using MathTestSystem.Domain.Entities;

namespace MathTestSystem.Application.Interfaces
{
    public interface IExamProcessingService
    {
        Task<ExamResultDto> ProcessExamAsync(Exam exam);
    }
}
