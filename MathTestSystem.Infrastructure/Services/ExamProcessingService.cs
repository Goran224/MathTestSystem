using AutoMapper;
using MathTestSystem.Application.DTOs;
using MathTestSystem.Application.Interfaces;
using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Enums;
using MathTestSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        var result = new ExamResultDto
        {
            ExternalExamId = exam.ExternalExamId
        };

        foreach (var task in exam.Tasks)
        {
            var expected = await _mathEvaluator.EvaluateAsync(task.Expression);

            var status = expected == task.SubmittedResult
                ? GradingStatus.Correct
                : GradingStatus.Incorrect;

            var taskResult = new TaskResult(
                task.Id,
                expected,
                task.SubmittedResult,
                status
            );

            task.AddTaskResult(taskResult);

            result.TaskResults.Add(new TaskResultDto
            {
                MathTaskId = task.Id,
                Expression = task.Expression,
                SubmittedResult = task.SubmittedResult,
                ExpectedResult = expected,
                Status = status
            });
        }

        return result;
    }
}
