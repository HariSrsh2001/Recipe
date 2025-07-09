namespace Food_Recipe.Domain.Models;

public class PendingUserRecipe
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required string Description { get; set; }
    public required string Img { get; set; }
    public int Rating { get; set; }
    public required string Ingredients { get; set; }
    public required string Instructions { get; set; }
    public bool IsApproved { get; set; }   
    public bool IsRejected { get; set; }  
    public DateTime CreatedAt { get; set; }
}
