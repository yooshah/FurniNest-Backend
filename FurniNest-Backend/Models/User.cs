using System.ComponentModel.DataAnnotations;

namespace FurniNest_Backend.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%*?&]{8,15}$",
            ErrorMessage = "Password must be 8 to 15 characters long and contain at least one letter, one number, and one special character.")]
        public string? Password { get; set; }

        [Required]
        public string? Role { get; set; }

        [Required]
        public bool AccountStatus { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;






    }
}
