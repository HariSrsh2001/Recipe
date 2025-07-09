using Food_Recipe.Domain.Models;

namespace Food_Recipe.Services.Interfaces;

public interface IAdminService
{
    List<User> GetAllUsers();
    List<PendingUserRecipe> GetPendingRecipes();
    List<UserRecipe> GetApprovedRecipes();
    PendingUserRecipe? GetPendingRecipe(int id);
    bool UpdatePendingRecipe(PendingUserRecipe updated);
    bool ApproveRecipe(int id);
    bool RejectRecipe(int id);
}
