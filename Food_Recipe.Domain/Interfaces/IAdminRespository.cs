using Food_Recipe.Domain.Models;

namespace Food_Recipe.Domain.Interfaces;

public interface IAdminRepository
{
    //Get a list of all users from the database
    List<User> GetAllUsers();

    //Get all recipes submitted by users that are waiting for approval
    List<PendingUserRecipe> GetPendingRecipes();

    //Get all recipes that have been approved by admin
    List<UserRecipe> GetApprovedRecipes();

    // Find a specific pending recipe by its ID (or return null if not found)
    PendingUserRecipe? GetPendingById(int id);

    //Update details of a pending recipe (e.g., edit name, description)
    void UpdatePending(PendingUserRecipe r);

    //Delete a pending recipe by its ID
    void DeletePending(int id);

    //Move a pending recipe to the approved recipes table
    void AddApproved(UserRecipe recipe);

    //Save all changes made to the database
    void Save();
}
