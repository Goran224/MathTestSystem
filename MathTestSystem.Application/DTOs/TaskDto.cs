using System.ComponentModel.DataAnnotations;

namespace MathTestSystem.Application.DTOs
{
    public class TaskDto
    {
        [Required]
        public string ExternalTaskId { get; set; } = string.Empty;

        [Required]
        public string Expression { get; set; } = string.Empty;

        [Required]
        public decimal SubmittedResult { get; set; }
    }
}
