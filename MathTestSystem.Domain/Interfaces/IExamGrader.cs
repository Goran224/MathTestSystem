using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Enums;

namespace MathTestSystem.Domain.Interfaces
{

    public interface IExamGrader
    {
        IReadOnlyCollection<GradingStatus> GradeExam(Exam exam);
    }
}
