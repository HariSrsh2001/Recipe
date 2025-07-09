// File: Food_Recipe.Services.Interfaces/IRecipeService.cs
using Food_Recipe.Domain.Models;
using Microsoft.AspNetCore.Http;

public interface IRecipeService
{
    /* Catalog */
    List<Recipe> FilterRecipes(string search, string category, int rating);
    List<Recipe> GetByCategory(string category);

    /* Favourite & Saved */
    List<Recipe> GetFavoritesByUsername(string username);
    List<Recipe> GetSavedByUsername(string username);
    void MarkFavorite(int recipeId, string username);
    void MarkSaved(int recipeId, string username);

    //void RemoveSavedRecipe(int recipeId, string username);

    //void RemoveFavoriteRecipe(int recipeId, string username);
    Task RemoveSavedRecipeAsync(int recipeId, string username);
    Task RemoveFavoriteRecipeAsync(int recipeId, string username);



    /* Pending recipe workflow */
    List<PendingUserRecipe> GetPendingRecipesByUser(string username);
    PendingUserRecipe? GetPendingRecipeById(int id);
    List<UserRecipe> GetApprovedRecipesByUser(string username);

   
    void SubmitUserRecipe(PendingUserRecipe newRecipe);
    void UpdatePendingRecipe(PendingUserRecipe updated);
    void DeleteUserPendingRecipe(int id, string username);

    /* Helpers */
    Dictionary<int, string> GetRecipeStatuses(IEnumerable<PendingUserRecipe> list);
    string SaveImage(IFormFile? file, string webRootPath);
}
