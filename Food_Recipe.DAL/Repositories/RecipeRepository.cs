// File: Food_Recipe.DAL.Repositories/RecipeRepository.cs
using Food_Recipe.DAL.Data;
using Food_Recipe.Domain.Interfaces;
using Food_Recipe.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Food_Recipe.DAL.Repositories;
public class RecipeRepository(FoodRecipeDbContext ctx) : IRecipeRepository
{
    private readonly FoodRecipeDbContext _ctx = ctx;

    /* -------- FoodRecipes -------- */

    //Returns all the recipes from the FoodRecipes table as a queryable object.
    public IQueryable<RecipeEntity> FoodRecipes() => _ctx.FoodRecipes.AsQueryable();

    //Fetch a specific recipe
    public RecipeEntity? GetFoodById(int id) => _ctx.FoodRecipes.Find(id);

    /* -------- Favourites / Saved -------- */

    //IDs of recipes the user marked as favorite
    public IEnumerable<int> GetFavoriteIds(string u) =>
        _ctx.FavoriteRecipes.Where(f => f.Username == u).Select(f => f.RecipeId).ToList();

    //IDs of recipes the user saved
    public IEnumerable<int> GetSavedIds(string u) =>
        _ctx.SavedRecipes.Where(s => s.Username == u).Select(s => s.RecipeId).ToList();


    //Mark recipe as favorite (if not already)
    public void AddFavorite(string u, int id)
    {
        if (!_ctx.FavoriteRecipes.Any(f => f.Username == u && f.RecipeId == id))
        {
            _ctx.FavoriteRecipes.Add(new FavoriteRecipe { Username = u, RecipeId = id, FavoritedAt = DateTime.Now });
            _ctx.SaveChanges();
        }
    }

    //Save recipe for later (if not already)
    public void AddSaved(string u, int id)
    {
        if (!_ctx.SavedRecipes.Any(s => s.Username == u && s.RecipeId == id))
        {
            _ctx.SavedRecipes.Add(new SavedRecipe { Username = u, RecipeId = id, SavedOn = DateTime.Now });
            _ctx.SaveChanges();
        }
    }


    //Remove a recipe from the user’s saved list
    public async Task RemoveFromSavedAsync(string username, int recipeId)
    {
        var saved = await _ctx.SavedRecipes
                              .FirstOrDefaultAsync(s => s.Username == username && s.RecipeId == recipeId);
        if (saved != null)
        {
            _ctx.SavedRecipes.Remove(saved);
            await _ctx.SaveChangesAsync();
        }
    }

    //	Remove a recipe from the user’s favorite list
    public async Task RemoveFromFavoriteAsync(string username, int recipeId)
    {
        var favorite = await _ctx.FavoriteRecipes
                                 .FirstOrDefaultAsync(f => f.Username == username && f.RecipeId == recipeId);
        if (favorite != null)
        {
            _ctx.FavoriteRecipes.Remove(favorite);
            await _ctx.SaveChangesAsync();
        }
    }




    /* -------- PendingUserRecipes -------- */

    //List user's submitted but unapproved recipes
    public IEnumerable<PendingUserRecipe> GetPending(string u) =>
        _ctx.PendingUserRecipes.Where(p => p.Username == u)
                               .OrderByDescending(p => p.CreatedAt).ToList();


    //	Get a specific pending recipe
    public PendingUserRecipe? GetPendingById(int id) =>
        _ctx.PendingUserRecipes.FirstOrDefault(p => p.Id == id);

    //Show user's approved (public) recipes
    public List<UserRecipe> GetApprovedRecipesByUser(string username)
    {
        return _ctx.UserRecipes
                   .Where(r => r.Username == username)
                   .OrderByDescending(r => r.CreatedAt)
                   .ToList();
    }

    //Add new pending recipe to DB
    public void AddPending(PendingUserRecipe r) { _ctx.PendingUserRecipes.Add(r); _ctx.SaveChanges(); }

    //	Modify an existing pending recipe
    public void UpdatePending(PendingUserRecipe r) { _ctx.PendingUserRecipes.Update(r); _ctx.SaveChanges(); }

    //Delete a pending recipe safely
    public void DeletePending(int id, string u)
    {
        var p = _ctx.PendingUserRecipes.FirstOrDefault(x => x.Id == id && x.Username == u && !x.IsApproved);
        if (p != null) { _ctx.PendingUserRecipes.Remove(p); _ctx.SaveChanges(); }
    }
}
