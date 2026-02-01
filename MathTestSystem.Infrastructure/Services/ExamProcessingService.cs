using MathTestSystem.Shared.DTOs;
using MathTestSystem.Shared.Interfaces;
using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Enums;
using MathTestSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

public class ExamProcessingService : IExamProcessingService
{
    private readonly IMathEvaluator _mathEvaluator;
    private readonly IExamRepository _examRepository;
    private readonly ILogger<ExamProcessingService> _logger;

    public ExamProcessingService(
        IMathEvaluator mathEvaluator,
        IExamRepository examRepository,
        ILogger<ExamProcessingService> logger)
    {
        _mathEvaluator = mathEvaluator;
        _examRepository = examRepository;
        _logger = logger;
    }

    public async Task<ExamResultDto> ProcessExamAsync(Exam exam)
    {
        var result = new ExamResultDto { ExternalExamId = exam.ExternalExamId };

        foreach (var task in exam.Tasks)
        {
            decimal expected = 0;
            try
            {
                expected = await _mathEvaluator.EvaluateAsync(task.Expression);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to evaluate task {TaskId}", task.Id);
            }

            var status = expected == task.SubmittedResult
                ? GradingStatus.Correct
                : GradingStatus.Incorrect;

            task.ExpectedResult = expected;
            task.Status = status;

            result.TaskResults.Add(new TaskResultDto
            {
                MathTaskId = task.Id,
                Expression = task.Expression,
                SubmittedResult = task.SubmittedResult,
                ExpectedResult = expected,
                Status = status
            });
        }

        try
        {
            await _examRepository.UpdateExamAsync(exam);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save exam grading info for {ExamId}", exam.Id);
        }

        return result;
    }
}