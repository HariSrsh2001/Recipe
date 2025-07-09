using Food_Recipe.Domain.Models;
namespace Food_Recipe.Domain.Interfaces;

public interface IRecipeRepository
{
    /* FoodRecipes table */
    IQueryable<RecipeEntity> FoodRecipes();           // returns an IQueryable for filtering
    RecipeEntity? GetFoodById(int id);

    /* Favourites & Saved */

    
    IEnumerable<int> GetFavoriteIds(string user);


    IEnumerable<int> GetSavedIds(string user);
    void AddFavorite(string user, int id);
    void AddSaved(string user, int id);

    //void RemoveFromSavedAsync(string user, int id);

    //void RemoveFromFavoriteAysnc(string username, int recipeId);
    Task RemoveFromFavoriteAsync(string username, int recipeId); // NEW
    Task RemoveFromSavedAsync(string username, int recipeId);


    /* PendingUserRecipes */

    //Get all recipes submitted by a user that are pending approval
    IEnumerable<PendingUserRecipe> GetPending(string user);

    //Get a specific pending recipe by ID
    PendingUserRecipe? GetPendingById(int id);

    //Add a new pending recipe submitted by a user
    void AddPending(PendingUserRecipe r);

    //Update a pending recipe (e.g., after user edits submission)
    void UpdatePending(PendingUserRecipe r);

    //Delete a pending recipe by its ID and user
    void DeletePending(int id, string user);

    //Get all approved recipes submitted by a specific user
    List<UserRecipe> GetApprovedRecipesByUser(string username);
}
