using MathTestSystem.Application.DTOs;
using MathTestSystem.Application.Interfaces;
using MathTestSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IExamProcessingService _examService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IExamProcessingService examService, ILogger<StudentsController> logger)
    {
        _examService = examService;
        _logger = logger;
    }

    [HttpPost("{studentId}/submit-exam")]
    [Authorize(Roles = "Teacher")] // only teachers can submit exams
    public async Task<IActionResult> SubmitExam(Guid studentId, [FromBody] ExamDto examDto)
    {
        try
        {
            if (examDto == null) return BadRequest("Exam data is required");

            // Map DTO to domain entity (AutoMapper or manual)
            var exam = new Exam(examDto.ExternalExamId);
            foreach (var t in examDto.Tasks)
                exam.AddTask(new MathTask(t.ExternalTaskId, t.Expression, t.SubmittedResult));

            var result = await _examService.ProcessExamAsync(exam);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process exam for student {StudentId}", studentId);
            return StatusCode(500, "An unexpected error occurred while processing the exam");
        }
    }

    [HttpGet("{studentId}/results")]
    [Authorize(Roles = "Student,Teacher")]
    public async Task<IActionResult> GetResults(Guid studentId)
    {
        try
        {
            // Fetch results from DB or service
            // Example:
            // var results = await _examService.GetResultsAsync(studentId);
            return Ok("stub: results here");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch results for student {StudentId}", studentId);
            return StatusCode(500, "An unexpected error occurred while fetching results");
        }
    }
}