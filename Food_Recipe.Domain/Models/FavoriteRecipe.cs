// FavoriteRecipe.cs
namespace Food_Recipe.Domain.Models;
public class FavoriteRecipe
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public int RecipeId { get; set; }

    public string RecipeName { get; set; } = string.Empty;

    public DateTime FavoritedAt { get; set; } = DateTime.Now;
}
