// MyRecipe.cs
namespace Food_Recipe.Domain.Models;
public class MyRecipe
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public int RecipeId { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.Now;
}
