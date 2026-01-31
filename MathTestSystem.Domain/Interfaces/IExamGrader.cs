using MathTestSystem.Domain.Entities;

namespace MathTestSystem.Domain.Interfaces
{

    public interface IExamGrader
    {
        IReadOnlyCollection<TaskResult> GradeExam(Exam exam);
    }
}
