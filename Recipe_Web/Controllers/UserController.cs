using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Food_Recipe.Domain.Models;
using Food_Recipe.Services.Interfaces;
using System.Threading.Tasks;
using System;

namespace Food_Recipe.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthService _auth;
        private readonly IRecipeService _recipes;
        private readonly IWebHostEnvironment _env;

        public UserController(IAuthService auth, IRecipeService recipes, IWebHostEnvironment env)
        {
            _auth = auth;
            _recipes = recipes;
            _env = env;
        }

        // ---------- Register ----------
        [HttpGet]
        //Show the Register Page
        public IActionResult Register() => View();

        [HttpPost]
            //Handle Registration Submission
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields.";
                return View(user);
            }

            bool exists = await _auth.UserExistsAsync(user.Username, user.Email);
            if (exists)
            {
                TempData["Error"] = "Username or Email already exists.";
                return View(user);
            }

            await _auth.RegisterAsync(user);
            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        // ---------- Login ----------
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string input, string password)
        {
            if (input == "admin" && password == "Admin@2025")
            {
                HttpContext.Session.SetString("Username", "admin");
                return RedirectToAction("Dashboard", "Admin");
            }

            var user = await _auth.LoginAsync(input, password);
            if (user == null)
            {
                ViewBag.Error = "Invalid username/email or password.";
                return View();
            }

            HttpContext.Session.SetString("Username", user.Username);
            TempData["Welcome"] = $"Welcome, {user.Username}!";
            return RedirectToAction("All", "Home");
        }

        // ---------- Logout ----------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        

        // ---------- Submit Recipe ----------
        [HttpPost]
        public IActionResult SubmitRecipe(IFormFile imageFile, string Name, string Category,
                                          string Ingredients, string Instructions, int Rating,
                                          string Description)
        {
            var user = HttpContext.Session.GetString("Username") ;
            //if (user == "Guest")
            //    return RedirectToAction("Login");

            var imgUrl = _recipes.SaveImage(imageFile, _env.WebRootPath);

            //creates a PendingUserRecipe object and submits it for approval.
            _recipes.SubmitUserRecipe(new PendingUserRecipe
            {
                Username = user,
                Name = Name,
                Category = Category,
                Ingredients = Ingredients,
                Instructions = Instructions,
                Rating = Rating,
                Description = Description,
                Img = imgUrl,
                CreatedAt = DateTime.Now,
                IsApproved = false,
                IsRejected = false
            });

            TempData["Success"] = "Recipe submitted for approval.";
            return RedirectToAction("MyRecipes", "Home");
        }
    }
}
