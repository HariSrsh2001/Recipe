using System.ComponentModel.DataAnnotations;

namespace Food_Recipe.Domain.Models
{
    
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        
    }
}
