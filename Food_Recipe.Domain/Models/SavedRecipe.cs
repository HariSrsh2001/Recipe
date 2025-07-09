using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Food_Recipe.Domain.Models
{
    public class SavedRecipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Username { get; set; }  // Or use UserId if you have it

        [Required]
        public int RecipeId { get; set; }

        public DateTime SavedOn { get; set; } = DateTime.Now;
    }
}
