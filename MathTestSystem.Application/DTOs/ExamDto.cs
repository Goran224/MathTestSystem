using System.ComponentModel.DataAnnotations;

namespace MathTestSystem.Shared.DTOs
{
    public class ExamDto
    {
        [Required]
        public string ExternalExamId { get; set; } = string.Empty;

        [Required]
        public string StudentExternalId { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public List<MathTaskDto> Tasks { get; set; } = new();
    }
}
