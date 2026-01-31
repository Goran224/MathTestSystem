namespace MathTestSystem.Application.DTOs
{
    public class ExamResultDto
    {
        public string ExternalExamId { get; set; } = string.Empty;
        public List<TaskResultDto> TaskResults { get; set; } = new();
    }
}
