using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Enums;
using MathTestSystem.Domain.Interfaces;
using MathTestSystem.Shared.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

public class ExamProcessingServiceTests
{
    [Fact]
    public async Task ProcessExamAsync_CorrectAnswer_SetsStatusCorrect()
    {
        var evaluator = new Mock<IMathEvaluator>();
        evaluator
            .Setup(x => x.EvaluateAsync("2+2"))
            .ReturnsAsync(4);

        var repo = new Mock<IExamRepository>();

        var service = new ExamProcessingService(
            evaluator.Object,
            repo.Object,
            NullLogger<ExamProcessingService>.Instance
        );

        var student = new Student("ST-1");

        var exam = new Exam("EX-1")
        {
            Student = student,
            StudentId = student.Id
        };

        exam.AddTask(new MathTask("1231","2+2", 4));

        var result = await service.ProcessExamAsync(exam);

        Assert.Single(result.TaskResults);
        Assert.Equal(GradingStatus.Correct, result.TaskResults[0].Status);
    }
}