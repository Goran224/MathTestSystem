namespace MathTestSystem.Application.DTOs
{
    public class ExamResultDto
    {
        public string ExternalExamId { get; set; } = string.Empty;

        private readonly List<TaskResultDto> _taskResults = new();
        public IReadOnlyCollection<TaskResultDto> TaskResults => _taskResults;

        public void AddTaskResult(TaskResultDto taskResult)
        {
            _taskResults.Add(taskResult);
        }
    }
}
