using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MathTestSystem.Infrastructure.Helpers
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(MathSystemDbContext context)
        {
            await context.Database.MigrateAsync(); // apply migrations if any

            if (!await context.Users.AnyAsync())
            {

                var teacher = new Teacher("11111");

                var studentA = new Student("54321");
                var studentB = new Student("12345");

                studentA.Teacher = teacher;
                studentB.Teacher = teacher;
                teacher.Students.Add(studentA);
                teacher.Students.Add(studentB);

                var exam1 = new Exam("24C41807-D220-4061-B0DB-3EA1E1EA5196");
                exam1.AddTask(new MathTask(Guid.NewGuid().ToString(), "2+3/6-4", 1.5m) { ExpectedResult = 1.5m, Status = GradingStatus.Correct });
                exam1.AddTask(new MathTask(Guid.NewGuid().ToString(), "5*3+1", 16m) { ExpectedResult = 16m, Status = GradingStatus.Correct });
                studentA.AddExam(exam1);

                var exam4 = new Exam("0E9EDE4A-55CC-4BA1-AE74-7837F53DA348");
                exam4.AddTask(new MathTask(Guid.NewGuid().ToString(), "1+1", 2m) { ExpectedResult = 2m, Status = GradingStatus.Correct});
                studentA.AddExam(exam4);
          
                var exam2 = new Exam("A0EF4583-66BE-4100-A3A9-5B8B7057A40D");
                exam2.AddTask(new MathTask(Guid.NewGuid().ToString(), "7-2", 5m) { ExpectedResult = 5m, Status = GradingStatus.Correct });
                studentB.AddExam(exam2);

                var examB2 = new Exam("B272A1D0-D309-420F-B866-6073AA03B3C5");
                examB2.AddTask(new MathTask(Guid.NewGuid().ToString(), "10 / 4", 2m) { ExpectedResult = 2.5m, Status = GradingStatus.Incorrect });
                studentB.AddExam(examB2);
                await context.Teachers.AddAsync(teacher);
                await context.SaveChangesAsync();
            }

            if (!await context.Users.AnyAsync(u => u.Username == "teacher1"))
            {
                var teacherEntity = await context.Teachers.FirstOrDefaultAsync(t => t.ExternalTeacherId == "11111");
                var teacherExternal = teacherEntity?.ExternalTeacherId ?? "11111";

                var teacherUser = new User("teacher1", HashPassword("password"), UserRole.Teacher)
                {
                    ExternalId = teacherExternal
                };
                await context.Users.AddAsync(teacherUser);
            }

            if (!await context.Users.AnyAsync(u => u.Username == "student1"))
            {
                var studentEntity = await context.Students.FirstOrDefaultAsync(s => s.ExternalStudentId == "54321");
                var studentExternal = studentEntity?.ExternalStudentId ?? "54321";

                var studentUser = new User("student1", HashPassword("password"), UserRole.Student)
                {
                    ExternalId = studentExternal
                };
                await context.Users.AddAsync(studentUser);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
