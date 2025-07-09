using Food_Recipe.Domain.Interfaces;
using Food_Recipe.Domain.Models;
using Food_Recipe.DAL.Data;
using System.Linq;

namespace Food_Recipe.DAL.Repositories;

public class AdminRepository(FoodRecipeDbContext ctx) : IAdminRepository
{
    private readonly FoodRecipeDbContext _ctx = ctx;

    //Display all users to the admin
    public List<User> GetAllUsers() => _ctx.Users.ToList();

    //Load recipes waiting for approval
    public List<PendingUserRecipe> GetPendingRecipes() =>
        _ctx.PendingUserRecipes.Where(x => !x.IsApproved && !x.IsRejected).ToList();

    //Show all user-approved recipes
    public List<UserRecipe> GetApprovedRecipes() =>
        _ctx.UserRecipes.OrderByDescending(r => r.CreatedAt).ToList();

    //Load a specific pending recipe
    public PendingUserRecipe? GetPendingById(int id) =>
        _ctx.PendingUserRecipes.FirstOrDefault(x => x.Id == id);

    //Mark a recipe as approved/rejected
    public void UpdatePending(PendingUserRecipe r)
    {
        _ctx.PendingUserRecipes.Update(r);
    }

    //Remove unwanted pending recipe
    public void DeletePending(int id)
    {
        var p = GetPendingById(id);
        if (p != null) _ctx.PendingUserRecipes.Remove(p);
    }

    //	Approve and move recipe to approved list
    public void AddApproved(UserRecipe recipe)
    {
        _ctx.UserRecipes.Add(recipe);
    }

    //Commit all above operations
    public void Save() => _ctx.SaveChanges();
}
