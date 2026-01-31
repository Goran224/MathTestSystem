using MathTestSystem.Application.Contracts;
using MathTestSystem.Application.DTOs;
using MathTestSystem.Application.Interfaces;
using MathTestSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MathTestSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IStudentParser _parser;
        private readonly IExamProcessingService _examService;

        public ExamController(IStudentParser parser, IExamProcessingService examService)
        {
            _parser = parser;
            _examService = examService;
        }

        [HttpPost("upload")]
        [Consumes("application/xml")]
        public async Task<IActionResult> UploadExam([FromBody] TeacherXml teacherXml)
        {
            if (teacherXml?.Students == null || !teacherXml.Students.Any())
                return BadRequest("No students found in XML");

            var results = new List<ExamResultDto>();

            foreach (var studentXml in teacherXml.Students) // ✅ direct list iteration
            {
                var student = XmlMapper.MapToStudent(studentXml); // map XML DTO to domain

                foreach (var exam in student.Exams)
                {
                    var result = await _examService.ProcessExamAsync(exam);
                    results.Add(result);
                }
            }

            return Ok(results);
        }
    }
}
