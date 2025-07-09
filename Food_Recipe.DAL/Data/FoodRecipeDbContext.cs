using Food_Recipe.Domain.Models; 
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Food_Recipe.DAL.Data
{
    public class FoodRecipeDbContext : DbContext
    {
        //sets up the database connection by passing configuration
        //options to the base DbContext class.
        public FoodRecipeDbContext(DbContextOptions<FoodRecipeDbContext> options) : base(options) { }
        public DbSet<RecipeEntity> FoodRecipes { get; set; }
        public DbSet<PendingUserRecipe> PendingUserRecipes { get; set; }
        public DbSet<UserRecipe> UserRecipes { get; set; }
        public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
        public DbSet<SavedRecipe> SavedRecipes { get; set; }
        public DbSet<User> Users { get; set; }

        //method that runs when your database model is being created.
        //It's where you can customize how your classes map to database tables.
        protected override void OnModelCreating(ModelBuilder mb)
        {
            
            mb.Entity<User>().ToTable("Users");//"Save the User class in a database table called Users.
            mb.Entity<RecipeEntity>().ToTable("FoodRecipes");
            mb.Entity<PendingUserRecipe>().ToTable("PendingUserRecipes");
            mb.Entity<UserRecipe>().ToTable("UserRecipes");
            mb.Entity<FavoriteRecipe>().ToTable("FavoriteRecipes");
            mb.Entity<SavedRecipe>().ToTable("SavedRecipes");
        }
    }
}
