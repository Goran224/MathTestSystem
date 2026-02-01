using MathTestSystem.Shared.DTOs;
using MathTestSystem.Shared.Interfaces;
using MathTestSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MathTestSystem.Infrastructure.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly MathSystemDbContext _context;
        private readonly ILogger<ExamRepository> _logger;

        public ExamRepository(MathSystemDbContext context, ILogger<ExamRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SaveTeacherTreeAsync(Teacher teacher)
        {
            try
            {
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error Save Teacher Tree Async student");
                throw;
            }
        }
        public async Task<List<ExamDto>> GetAllExamsAsync()
        {
            try
            {
                var exams = await _context.Exams
                    .Include(e => e.Tasks)
                    .Include(e => e.Student)
                        .ThenInclude(s => s.Teacher)
                    .ToListAsync();

                var examDtos = exams.Select(e => new ExamDto
                {
                    ExternalExamId = e.ExternalExamId,
                    Tasks = e.Tasks.Select(t => new MathTaskDto
                    {
                        ExpectedResult = t.ExpectedResult,
                        Expression = t.Expression,
                        SubmittedResult = t.SubmittedResult,
                        Status = t.Status
                    }).ToList()
                }).ToList();

                return examDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllExamsAsync");
                throw;
            }
        }

        public async Task<List<ExamDto>> GetExamsByStudentAsync(string externalStudentId)
        {
            try
            {
                var exams = await _context.Exams
                    .Include(e => e.Tasks)
                    .Include(e => e.Student)
                        .ThenInclude(s => s.Teacher)
                    .Where(e => e.Student.ExternalStudentId == externalStudentId)
                    .ToListAsync();

                return exams.Select(e => new ExamDto
                {
                    ExternalExamId = e.ExternalExamId,
                    Tasks = e.Tasks.Select(t => new MathTaskDto
                    {
                        ExpectedResult = t.ExpectedResult,
                        Expression = t.Expression,
                        SubmittedResult = t.SubmittedResult,
                        Status = t.Status
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetExamsByStudentAsync");
                throw;
            }
        }

        public async Task<List<ExamDto>> GetExamsByTeacherAsync(string externalTeacherId)
        {
            try
            {
                var exams = await _context.Exams
                    .Include(e => e.Tasks)
                    .Include(e => e.Student)
                        .ThenInclude(s => s.Teacher)
                    .Where(e => e.Student.Teacher.ExternalTeacherId == externalTeacherId)
                    .ToListAsync();

                return exams.Select(e => new ExamDto
                {
                    ExternalExamId = e.ExternalExamId,
                    Tasks = e.Tasks.Select(t => new MathTaskDto
                    {
                        ExpectedResult = t.ExpectedResult,
                        Expression = t.Expression,
                        SubmittedResult = t.SubmittedResult,
                        Status = t.Status
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetExamsByTeacherAsync");
                throw;
            }
        }

        public async Task UpdateExamAsync(Exam exam)
        {
            try
            {
                var trackedExam = await _context.Exams
                    .Include(e => e.Tasks)
                    .FirstOrDefaultAsync(e => e.Id == exam.Id);

                if (trackedExam != null)
                {
                    foreach (var task in exam.Tasks)
                    {
                        var trackedTask = trackedExam.Tasks.FirstOrDefault(t => t.Id == task.Id);
                        if (trackedTask != null)
                        {
                            trackedTask.ExpectedResult = task.ExpectedResult;
                            trackedTask.Status = task.Status;
                        }
                    }
                }
                else
                {
                    _context.Exams.Update(exam);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exam {ExamId}", exam.Id);
                throw;
            }
        }
    }
}
