// Services/AuthService.cs
using Food_Recipe.Domain.Models;
using Food_Recipe.Domain.Interfaces;
using Food_Recipe.Services.Interfaces;
using System.Threading.Tasks;

namespace Food_Recipe.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;

        public AuthService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            return await _repo.UserExistsAsync(username, email);
        }

        public async Task RegisterAsync(User user)
        {
            await _repo.AddAsync(user);
        }

        //Logs in the user by matching their username/email and password.
        //Returns user if match found, else null.
        public async Task<User?> LoginAsync(string input, string password)
        {
            var user = await _repo.FindForLoginAsync(input);
            return user != null && user.Password == password ? user : null;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _repo.GetByEmailAsync(email);
        }
    }
}
