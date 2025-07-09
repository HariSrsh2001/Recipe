using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Food_Recipe.Domain.Interfaces;
using Food_Recipe.Domain.Models;
using Food_Recipe.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Food_Recipe.Services.Implementations
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repo;

        public RecipeService(IRecipeRepository repo) => _repo = repo;

        /* ------------ Catalog ------------ */
        public List<Recipe> FilterRecipes(string search, string category, int minRating)
        {
            var query = _repo.FoodRecipes();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(r => r.Name.ToLower().Contains(lowerSearch) ||
                                         r.Description.ToLower().Contains(lowerSearch));
            }

            if (!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                var lowerCategory = category.ToLower();
                query = query.Where(r => r.Category.ToLower() == lowerCategory);
            }

            if (minRating > 0)
            {
                query = query.Where(r => r.Rating >= minRating);
            }

            return query.ToList().Select(Convert).ToList();
        }



        public List<Recipe> GetByCategory(string category) =>
    _repo.FoodRecipes()
         .Where(r => r.Category.ToLower() == category.ToLower())
         .Select(Convert)
         .ToList();


        /* ------------ Favourites & Saved ------------ */
        public List<Recipe> GetFavoritesByUsername(string username) =>
            _repo.FoodRecipes()
                 .Where(r => _repo.GetFavoriteIds(username).Contains(r.Id))
                 .ToList()
                 .Select(Convert)
                 .ToList();
        //convert -method that transforms a RecipeEntity object (from the database)
        //into a Recipe object (used in your application/UI).

        public List<Recipe> GetSavedByUsername(string username) =>
            _repo.FoodRecipes()
                 .Where(r => _repo.GetSavedIds(username).Contains(r.Id))
                 .ToList()
                 .Select(Convert)
                 .ToList();

        //Mark->this recipe as favorite for the user
        //Add->Add this record to the FavoriteRecipes table
        public void MarkFavorite(int recipeId, string username) => _repo.AddFavorite(username, recipeId);
        public void MarkSaved(int recipeId, string username) => _repo.AddSaved(username, recipeId);

    //    

        public async Task RemoveSavedRecipeAsync(int recipeId, string username)
        {
            await _repo.RemoveFromSavedAsync(username, recipeId);
        }

        public async Task RemoveFavoriteRecipeAsync(int recipeId, string username)
        {
            await _repo.RemoveFromFavoriteAsync(username, recipeId);
        }




        /* ------------ Pending workflow ------------ */
        public List<PendingUserRecipe> GetPendingRecipesByUser(string username) =>
            _repo.GetPending(username).ToList();

        public PendingUserRecipe? GetPendingRecipeById(int id) => _repo.GetPendingById(id);

        public List<UserRecipe> GetApprovedRecipesByUser(string username)
        {
            return _repo.GetApprovedRecipesByUser(username);
        }


        public void SubmitUserRecipe(PendingUserRecipe recipe) => _repo.AddPending(recipe);
        public void UpdatePendingRecipe(PendingUserRecipe recipe) => _repo.UpdatePending(recipe);
        public void DeleteUserPendingRecipe(int id, string user) => _repo.DeletePending(id, user);

        public Dictionary<int, string> GetRecipeStatuses(IEnumerable<PendingUserRecipe> list) =>
            list.ToDictionary(
                x => x.Id,
                x => x.IsApproved ? "Approved"
                     : x.IsRejected ? "Rejected"
                     : "Pending");

        /* ------------ Util ------------ */
        public string SaveImage(IFormFile? file, string webRootPath)
        {
            if (file == null || file.Length == 0)
                return "/images/default.jpg";

            string uploads = Path.Combine(webRootPath, "uploads");
            Directory.CreateDirectory(uploads);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string fullPath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return $"/uploads/{fileName}";
        }

        /* ------------ Mapping ------------ */
        private static Recipe Convert(RecipeEntity r)
        {
            return new()
            {
                Id = r.Id,
                Name = r.Name,
                Category = r.Category,
                Description = r.Description,
                Img = r.Img,
                Rating = r.Rating,
                Ingredients = JsonConvert.DeserializeObject<List<string>>(r.Ingredients ?? "[]") ?? new List<string>(),
                Instructions = JsonConvert.DeserializeObject<List<string>>(r.Instructions ?? "[]") ?? new List<string>(),
                Nutrition = JsonConvert.DeserializeObject<Nutrition>(r.Nutrition ?? "{}") ?? new Nutrition()

            };
        }
    }
}
