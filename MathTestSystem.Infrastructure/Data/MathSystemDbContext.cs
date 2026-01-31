using MathTestSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class MathSystemDbContext : DbContext
{
    public MathSystemDbContext(DbContextOptions<MathSystemDbContext> options)
        : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<MathTask> MathTasks => Set<MathTask>();
    public DbSet<TaskResult> TaskResults => Set<TaskResult>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Student -> Exams
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.ExternalStudentId).IsRequired();
            entity.HasMany(s => s.Exams)
                  .WithOne(e => e.Student)
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Exam -> Tasks
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExternalExamId).IsRequired();
            entity.HasMany(e => e.Tasks)
                  .WithOne(t => t.Exam)
                  .HasForeignKey(t => t.ExamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // MathTask -> TaskResults
        modelBuilder.Entity<MathTask>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.ExternalTaskId).IsRequired();
            entity.Property(t => t.Expression).IsRequired();
            entity.Property(t => t.SubmittedResult).IsRequired();

            entity.HasMany(t => t.TaskResults)
                  .WithOne(tr => tr.MathTask)
                  .HasForeignKey(tr => tr.MathTaskId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TaskResult
        modelBuilder.Entity<TaskResult>(entity =>
        {
            entity.HasKey(tr => tr.Id);
            entity.Property(tr => tr.MathTaskId).IsRequired();
            entity.Property(tr => tr.ExpectedResult).IsRequired();
            entity.Property(tr => tr.SubmittedResult).IsRequired();
            entity.Property(tr => tr.Status).IsRequired();
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired();
        });
    }
}