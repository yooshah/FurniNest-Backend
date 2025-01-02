using System.ComponentModel.DataAnnotations;

namespace FurniNest_Backend.DTOs.UserDTOs
{
    public class UserRegisterDTO
    {

        [Required]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%*?&]{8,15}$",
          ErrorMessage = "Password must be 8 to 15 characters long and contain at least one letter, one number, and one special character.")]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string? confirmPassword { get; set; }

    }
}
