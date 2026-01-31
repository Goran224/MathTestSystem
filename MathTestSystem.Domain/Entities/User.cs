using MathTestSystem.Domain.Enums;

namespace MathTestSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // store hashed password
        public UserRole Role { get; set; } // Teacher or Student

        protected User() { } // EF Core

        public User(string username, string passwordHash, UserRole role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
