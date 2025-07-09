using Food_Recipe.DAL.Data;
using Food_Recipe.DAL.Repositories;
//using Food_Recipe.Data;
using Food_Recipe.Domain.Interfaces;
using Food_Recipe.Services.Implementations;
using Food_Recipe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add ApplicationDbContext
builder.Services.AddDbContext<FoodRecipeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services and repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IAdminService, AdminService>();


// Add MVC and session
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");



app.Run();
