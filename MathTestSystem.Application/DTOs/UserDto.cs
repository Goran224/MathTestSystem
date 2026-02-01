using MathTestSystem.Domain.Enums;

namespace MathTestSystem.Shared.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public string? ExternalId { get; set; }
    }
}