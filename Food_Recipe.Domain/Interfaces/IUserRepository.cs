// Domain/Interfaces/IUserRepository.cs
using Food_Recipe.Domain.Models;
using System.Threading.Tasks;

namespace Food_Recipe.Domain.Interfaces
{
    public interface IUserRepository
    {
        //Check if a user already exists by username or email
        Task<bool> UserExistsAsync(string username, string email);

        //Add a new user to the database
        Task AddAsync(User user);

        //Get user by username or email for login
        Task<User?> FindForLoginAsync(string input); // username or email

        //Get user details using email
        Task<User?> GetByEmailAsync(string email);
        

    }
}
