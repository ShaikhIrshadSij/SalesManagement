using SalesManagement.API.Models;

namespace SalesManagement.API.Interfaces
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<string> GenerateJwtTokenAsync(User user);
    }
}
