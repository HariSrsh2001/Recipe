using System.ComponentModel.DataAnnotations;

namespace Food_Recipe.Domain.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public required string Category { get; set; }
        public required string Name { get; set; }
        public required string Img { get; set; }
        public required string Description { get; set; }
        public int Rating { get; set; }
        public required Nutrition Nutrition { get; set; }
        public required List<string> Ingredients { get; set; }
        public required List<string> Instructions { get; set; }
    }
    public class Nutrition
    {

        public int Calories { get; set; }
        public  string Protein { get; set; }
        public  string Carbs { get; set; }
        public  string Fat { get; set; }
    }
}
