using MathTestSystem.Domain.Enums;

namespace MathTestSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        // Single external identifier that maps the user to a domain Student or Teacher
        public string? ExternalId { get; set; }

        protected User() { }

        public User(string username, string passwordHash, UserRole role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }
    }   
}
