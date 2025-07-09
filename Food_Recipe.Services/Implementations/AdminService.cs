using Food_Recipe.Domain.Models;
using Food_Recipe.Domain.Interfaces;
using Food_Recipe.Services.Interfaces;

namespace Food_Recipe.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _repo;
    public AdminService(IAdminRepository repo)
    {
        _repo = repo;
    }

    public List<User> GetAllUsers() => _repo.GetAllUsers();

    public List<PendingUserRecipe> GetPendingRecipes() => _repo.GetPendingRecipes();

    public List<UserRecipe> GetApprovedRecipes() => _repo.GetApprovedRecipes();

    public PendingUserRecipe? GetPendingRecipe(int id) => _repo.GetPendingById(id);

    public bool UpdatePendingRecipe(PendingUserRecipe form)
    {
        var r = _repo.GetPendingById(form.Id);
        if (r == null || r.IsApproved) return false;

        r.Name = form.Name;
        r.Category = form.Category;
        r.Description = form.Description;
        r.Ingredients = form.Ingredients;
        r.Instructions = form.Instructions;
        r.Rating = form.Rating;
        r.IsRejected = false;

        _repo.UpdatePending(r);
        _repo.Save();
        return true;
    }

    public bool ApproveRecipe(int id)
    {
        var p = _repo.GetPendingById(id);
        if (p == null || p.IsApproved) return false;

        var r = new UserRecipe
        {
            Username = p.Username,
            Name = p.Name,
            Category = p.Category,
            Img = p.Img ?? "",
            Description = p.Description ?? "",
            Rating = p.Rating,
            Ingredients = p.Ingredients ?? "[]",
            Instructions = p.Instructions ?? "[]",
            CreatedAt = DateTime.Now
        };

        _repo.AddApproved(r);
        _repo.DeletePending(p.Id);
        _repo.Save();
        return true;
    }

    public bool RejectRecipe(int id)
    {
        var p = _repo.GetPendingById(id);
        if (p == null || p.IsApproved) return false;

        p.IsRejected = true;
        _repo.UpdatePending(p);
        _repo.Save();
        return true;
    }
}
