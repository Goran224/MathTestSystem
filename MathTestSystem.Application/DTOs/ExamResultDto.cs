namespace MathTestSystem.Shared.DTOs
{
    public class ExamResultDto
    {
        public string ExternalExamId { get; set; } = string.Empty;
        public List<TaskResultDto> TaskResults { get; set; } = new();
    }
}
