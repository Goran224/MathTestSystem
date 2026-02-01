using MathTestSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class MathSystemDbContext : DbContext
{
    public MathSystemDbContext(DbContextOptions<MathSystemDbContext> options)
        : base(options) { }

    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<MathTask> MathTasks => Set<MathTask>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Teacher>()
            .HasMany(t => t.Students)
            .WithOne(s => s.Teacher)
            .HasForeignKey(s => s.TeacherId);

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.ExternalStudentId).IsRequired();
            entity.HasMany(s => s.Exams)
                  .WithOne(e => e.Student)
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExternalExamId).IsRequired();
            entity.HasMany(e => e.Tasks)
                  .WithOne(t => t.Exam)
                  .HasForeignKey(t => t.ExamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MathTask>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.ExternalTaskId).IsRequired();
            entity.Property(t => t.Expression).IsRequired();
            entity.Property(t => t.SubmittedResult)
                  .HasColumnType("decimal(10,2)")   // <-- round to 2 decimals
                  .IsRequired();
            entity.Property(t => t.ExpectedResult)
                  .HasColumnType("decimal(10,2)")   // <-- round to 2 decimals
                  .IsRequired();
        });


        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired();
        });

        modelBuilder.Entity<MathTask>(entity =>
        {
            entity.Property(t => t.SubmittedResult)
                  .HasPrecision(18, 6); 
        });
    }
}