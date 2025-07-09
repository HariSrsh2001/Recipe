// DAL/Repositories/UserRepository.cs
using Food_Recipe.DAL.Data;
using Food_Recipe.Domain.Interfaces;
using Food_Recipe.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace Food_Recipe.DAL.Repositories
{
    public class UserRepository(FoodRecipeDbContext context) : IUserRepository
    {

        //stores the database connection inside the class so we can use
        //it later to add users, check if they exist,
        //or get user info from the database.
        private readonly FoodRecipeDbContext _context = context;

        //Check if a user already exists
        public async Task<bool> UserExistsAsync(string username, string email)
        {
            return await _context.Users.AnyAsync(u => u.Username == username || u.Email == email);
        }

        //Add/register a new user
        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        //Login using username or email
        public async Task<User?> FindForLoginAsync(string input)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == input || u.Email == input);
        }

        //Find user by email for reset/profile/validation
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        

    }
}
