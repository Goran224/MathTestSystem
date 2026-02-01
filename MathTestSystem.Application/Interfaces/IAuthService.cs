using MathTestSystem.Shared.DTOs;

namespace MathTestSystem.Shared.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password);

        Task<UserDto?> GetUserByUsernameAsync(string username);
    }
}
