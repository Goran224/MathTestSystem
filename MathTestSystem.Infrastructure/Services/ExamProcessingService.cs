using MathTestSystem.Application.DTOs;
using MathTestSystem.Application.Interfaces;
using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Enums;
using MathTestSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

public class ExamProcessingService : IExamProcessingService
{
    private readonly IMathEvaluator _mathEvaluator;
    private readonly ILogger<ExamProcessingService> _logger;

    public ExamProcessingService(IMathEvaluator mathEvaluator, ILogger<ExamProcessingService> logger)
    {
        _mathEvaluator = mathEvaluator;
        _logger = logger;
    }

    public async Task<ExamResultDto> ProcessExamAsync(Exam exam)
    {
        if (exam == null) throw new ArgumentNullException(nameof(exam));

        var resultDto = new ExamResultDto { ExternalExamId = exam.ExternalExamId };

        try
        {
            foreach (var task in exam.Tasks)
            {
                decimal expected;
                try
                {
                    expected = await _mathEvaluator.EvaluateAsync(task.Expression);
                }
                catch (Exception evalEx)
                {
                    _logger.LogWarning(evalEx, "Failed to evaluate task {TaskId}", task.Id);
                    expected = 0;
                }

                var taskResultDto = new TaskResultDto
                {
                    MathTaskId = task.Id,
                    ExpectedResult = expected,
                    Status = expected == task.SubmittedResult ? GradingStatus.Correct : GradingStatus.Incorrect
                };

                resultDto.AddTaskResult(taskResultDto);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical error processing exam {ExamId}", exam.ExternalExamId);
            throw;
        }

        return resultDto;
    }
}