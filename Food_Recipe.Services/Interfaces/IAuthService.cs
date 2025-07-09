// Services/Interfaces/IAuthService.cs
using Food_Recipe.Domain.Models;
using System.Threading.Tasks;

namespace Food_Recipe.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> UserExistsAsync(string username, string email);
        Task RegisterAsync(User user);
        Task<User?> LoginAsync(string input, string password);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
