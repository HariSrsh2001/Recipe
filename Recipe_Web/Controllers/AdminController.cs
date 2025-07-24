using Microsoft.AspNetCore.Mvc;
using Food_Recipe.Services.Interfaces;
using Food_Recipe.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Food_Recipe.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _admin;

        public AdminController(IAdminService admin)
        {
            _admin = admin;
        }

        // Utility method to check admin session
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Username") == "admin" &&
                   HttpContext.Session.GetString("IsAdmin") == "true";
        }

        // Show all users
        public IActionResult Users()
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden);

            var users = _admin.GetAllUsers();
            return View(users);
        }

        // Show approved or pending recipes
        public IActionResult Dashboard(string status = "approved")
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden);

            status = (status ?? "approved").ToLower();
            if (status == "pending")
                return View("Dashboard", _admin.GetPendingRecipes());

            return View("Dashboard", _admin.GetApprovedRecipes());
        }

        [HttpGet]
        // Show edit form for a pending recipe
        public IActionResult EditRecipe(int id)
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden);

            var rec = _admin.GetPendingRecipe(id);
            if (rec == null || rec.IsApproved)
                return RedirectToAction("Dashboard", new { status = "pending" });

            return View(rec);
        }

     
        [HttpPost]
        // Save admin-edited recipe with optional image upload
        public async Task<IActionResult> EditRecipe(PendingUserRecipe form, IFormFile imageFile)
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden);

            var recipe = _admin.GetPendingRecipe(form.Id);
            if (recipe == null || recipe.IsApproved)
                return RedirectToAction("Dashboard", new { status = "pending" });

            // Update all editable fields using a helper
            UpdateRecipeFields(recipe, form);

            // Update image only if new image provided
            if (imageFile?.Length > 0)
            {
                recipe.Img = await SaveImageAsync(imageFile);
            }

            TempData["Message"] = _admin.UpdatePendingRecipe(recipe)
                ? "Changes saved."
                : "Update failed.";

            return RedirectToAction("Dashboard", new { status = "pending" });
        }

        // Helper to update basic fields
        private void UpdateRecipeFields(PendingUserRecipe target, PendingUserRecipe source)
        {
            target.Name = source.Name;
            target.Description = source.Description;
            target.Category = source.Category;
            target.Ingredients = source.Ingredients;
            target.Instructions = source.Instructions;
            target.Rating = source.Rating;
        }

        // Helper to save image and return path
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return "/images/" + uniqueFileName;
        }



        [HttpPost]
        // Approve a recipe
        public IActionResult Approve(int id)
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden);

            _admin.ApproveRecipe(id);
            return RedirectToAction("Dashboard", new { status = "approved" });
        }

        [HttpPost]
        // Reject a recipe
        public IActionResult Reject(int id)
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden);

            _admin.RejectRecipe(id);
            return RedirectToAction("Dashboard", new { status = "pending" });
        }

        // Log out admin (clears session)
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}
