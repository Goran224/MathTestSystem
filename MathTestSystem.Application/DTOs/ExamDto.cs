using System.ComponentModel.DataAnnotations;

namespace MathTestSystem.Application.DTOs
{
    public class ExamDto
    {
        [Required]
        public string ExternalExamId { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public List<TaskDto> Tasks { get; set; } = new();
    }
}
