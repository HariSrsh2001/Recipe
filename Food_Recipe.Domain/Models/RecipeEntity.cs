using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Food_Recipe.Domain.Models
{
    public class RecipeEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Category { get; set; }

        [Required]
        public required string Name { get; set; }

        public required string Img { get; set; }
        public required string Description { get; set; }
        public int Rating { get; set; }
        public required string Nutrition { get; set; }
        public required string Ingredients { get; set; }
        public required string Instructions { get; set; }

        
    }
}