using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Food_Recipe.Domain.Models;
using Food_Recipe.Services.Interfaces;

namespace Food_Recipe.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRecipeService _service;
        private readonly IWebHostEnvironment _env;

        public HomeController(IRecipeService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        /* ------------------ Catalog ------------------ */
        public IActionResult All(string search = "", string category = "All", int rating = 0)
        {
            var domainList = _service.FilterRecipes(search, category, rating);
            var viewModels = domainList.Select(MapToViewModel).ToList();
            return View("All", viewModels);
        }

        public IActionResult Veg() => View("Veg", _service.GetByCategory("veg").Select(MapToViewModel).ToList());
        public IActionResult NonVeg() => View("NonVeg", _service.GetByCategory("nonveg").Select(MapToViewModel).ToList());
        public IActionResult Beverages() => View("Beverages", _service.GetByCategory("beverages").Select(MapToViewModel).ToList());

        /* ------------------ Favourites & Saved ------------------ */
        public IActionResult Favorite()
        {
            var username = UserName();
            var viewModels = _service.GetFavoritesByUsername(username).Select(MapToViewModel).ToList();
            return View("Favorite", viewModels);
        }

        public IActionResult Saved()
        {
            var username = UserName();
            var viewModels = _service.GetSavedByUsername(username).Select(MapToViewModel).ToList();
            return View("Saved", viewModels);
        }

        /* ------------------ My Recipes ------------------ */
        public IActionResult MyRecipes()
        {
            var username = UserName();
            var pendingDomain = _service.GetPendingRecipesByUser(username);
            var approvedDomain = _service.GetApprovedRecipesByUser(username);

            ViewBag.Status = _service.GetRecipeStatuses(pendingDomain);
            ViewBag.ApprovedRecipes = approvedDomain;

            return View("MyRecipes", pendingDomain);
        }

        [HttpPost]
        public IActionResult AddMyRecipe(string name, string category, string description,
                                         int rating, string ingredients, string instructions,
                                         IFormFile imageFile)
        {
            var user = UserName();
            if (user == "Guest")
                return RedirectToAction("Login", "User");

            var imgUrl = _service.SaveImage(imageFile, _env.WebRootPath);

            var rec = new PendingUserRecipe
            {
                Username = user,
                Name = name,
                Category = category,
                Description = description,
                Rating = rating,
                Img = imgUrl,
                Ingredients = ingredients,
                Instructions = instructions,
                CreatedAt = DateTime.Now,
                IsApproved = false,
                IsRejected = false
            };

            _service.SubmitUserRecipe(rec);
            TempData["Message"] = "Recipe submitted and is waiting for admin approval.";
            return RedirectToAction("MyRecipes");
        }

        /* ------------------ Edit ------------------ */
        public IActionResult EditRecipe(int id)
        {
            var recipe = _service.GetPendingRecipeById(id);
            if (recipe == null || recipe.IsApproved) return NotFound();
            return View(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecipe(PendingUserRecipe updated, IFormFile imageFile)
        {
            var existing = _service.GetPendingRecipeById(updated.Id);
            if (existing == null || existing.IsApproved) return NotFound();

            existing.Name = updated.Name;
            existing.Category = updated.Category;
            existing.Description = updated.Description;
            existing.Rating = updated.Rating;
            existing.Ingredients = updated.Ingredients;
            existing.Instructions = updated.Instructions;

            // Handle new image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                existing.Img = "/images/" + uniqueFileName;
            }

            _service.UpdatePendingRecipe(existing);
            TempData["Message"] = "Recipe updated successfully.";
            return RedirectToAction("MyRecipes");
        }

        [HttpPost]
        public IActionResult SubmitForApproval(int id)
        {
            var recipe = _service.GetPendingRecipeById(id);
            if (recipe == null || recipe.IsApproved)
                return NotFound();

            recipe.IsRejected = false;
            _service.UpdatePendingRecipe(recipe);

            TempData["Message"] = "Recipe submitted for admin approval.";
            return RedirectToAction("MyRecipes");
        }

        /* ------------------ Delete ------------------ */
        [HttpPost]
        public IActionResult DeleteMyRecipe(int id)
        {
            _service.DeleteUserPendingRecipe(id, UserName());
            return RedirectToAction("MyRecipes");
        }

        /* ------------------ Favourite / Save ------------------ */
        [HttpPost] public IActionResult AddToFavorite(int id) => AddMark(id, true);
        [HttpPost] public IActionResult AddToSaved(int id) => AddMark(id, false);

        [HttpPost]
        public async Task<IActionResult> RemoveSaved(int id)
        {
            var username = UserName();
            await _service.RemoveSavedRecipeAsync(id, username);
            return RedirectToAction("Saved");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            var username = UserName();
            await _service.RemoveFavoriteRecipeAsync(id, username);
            return RedirectToAction("Favorite");
        }

        /* ------------------ Helpers ------------------ */
        private IActionResult AddMark(int id, bool fav)
        {
            var username = UserName();
            if (username == "Guest") return RedirectToAction("All");

            if (fav) _service.MarkFavorite(id, username);
            else _service.MarkSaved(id, username);

            return Redirect(Request.Headers["Referer"].ToString() ?? "/");
        }

        private string UserName() =>
            HttpContext.Session.GetString("Username") ?? "Guest";

        private static Recipe MapToViewModel(Recipe r)
        {
            return new()
            {
                Id = r.Id,
                Name = r.Name,
                Category = r.Category,
                Ingredients = r.Ingredients,
                Instructions = r.Instructions,
                Description = r.Description,
                Img = r.Img,
                Rating = r.Rating,
                Nutrition = r.Nutrition
            };
        }
    }
}
