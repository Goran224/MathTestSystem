using MathTestSystem.Shared.Contracts;
using MathTestSystem.Shared.DTOs;
using MathTestSystem.Shared.Interfaces;
using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Interfaces;
using MathTestSystem.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ExamController : ControllerBase
{
    private readonly IStudentParser _parser;
    private readonly IExamProcessingService _examService;
    private readonly IExamRepository _examRepository;
    private readonly ILogger<ExamController> _logger;

    public ExamController(
        IStudentParser parser,
        IExamProcessingService examService,
        IExamRepository examRepository,
        ILogger<ExamController> logger)
    {
        _parser = parser;
        _examService = examService;
        _examRepository = examRepository;
        _logger = logger;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Teacher")]
    [Consumes("application/xml")]
    public async Task<IActionResult> UploadExam([FromBody] TeacherXml teacherXml)
    {
        try
        {
            if (teacherXml?.Students == null || !teacherXml.Students.Any())
                return BadRequest("No students found in XML");

            var teacher = new Teacher(teacherXml.ID);

            foreach (var studentXml in teacherXml.Students)
            {
                try
                {
                    var student = XmlMapper.MapToStudent(studentXml);
                    student.Teacher = teacher;
                    teacher.Students.Add(student);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error mapping student {StudentId}", studentXml.ID);
                }
            }

            try
            {
                await _examRepository.SaveTeacherTreeAsync(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving teacher tree {TeacherId}", teacher.Id);
                throw;
            }

            var results = new List<ExamResultDto>();
            foreach (var student in teacher.Students)
            {
                foreach (var exam in student.Exams)
                {
                    try
                    {
                        var result = await _examService.ProcessExamAsync(exam);
                        results.Add(result);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing exam {ExamId}", exam.ExternalExamId);
                    }
                }
            }

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error uploading exams");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllExams()
    {
        try
        {
            var exams = await _examRepository.GetAllExamsAsync();
            return Ok(exams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all exams");
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [HttpGet("student/{externalStudentId}")]
    public async Task<IActionResult> GetExamsByStudent(string externalStudentId)
    {
        try
        {
            var exams = await _examRepository.GetExamsByStudentAsync(externalStudentId);
            return Ok(exams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching exams for student {StudentId}", externalStudentId);
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [HttpGet("teacher/{externalTeacherId}")]
    public async Task<IActionResult> GetExamsByTeacher(string externalTeacherId)
    {
        try
        {
            var exams = await _examRepository.GetExamsByTeacherAsync(externalTeacherId);
            return Ok(exams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching exams for teacher {TeacherId}", externalTeacherId);
            return StatusCode(500, "An unexpected error occurred");
        }
    }
}