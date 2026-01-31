using System.ComponentModel.DataAnnotations;

namespace MathTestSystem.Application.DTOs
{
    public class StudentDto
    {
        [Required]
        public string ExternalStudentId { get; set; } = string.Empty;

        [Required]
        public List<ExamDto> Exams { get; set; } = new();
    }
}
