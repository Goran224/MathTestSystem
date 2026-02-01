using System.ComponentModel.DataAnnotations;

namespace MathTestSystem.Shared.DTOs
{
    public class StudentDto
    {
        [Required]
        public string ExternalStudentId { get; set; } = string.Empty;

        [Required]
        public List<ExamDto> Exams { get; set; } = new();
    }
}
