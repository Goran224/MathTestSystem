using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                var teacher = new User("teacher1", HashPassword("password"), UserRole.Teacher);
                var student = new User("student1", HashPassword("password"), UserRole.Student);

                await context.Users.AddRangeAsync(teacher, student);
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
