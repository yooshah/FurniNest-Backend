using System.ComponentModel.DataAnnotations;

namespace FurniNest_Backend.DTOs.OrderDTOs
{
    public class OrderAddressDTO
    {
        

        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is required")]

        [MaxLength(70, ErrorMessage = "Address cannot exceed more than 70 characters!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]

        [MaxLength(20, ErrorMessage = "City must not exceed 20 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "District is required")]

        [MaxLength(20, ErrorMessage = "District must not exceed more than 20 characters")]
        public string District { get; set; }

        [Required(ErrorMessage = "State is required")]

        [MaxLength(20, ErrorMessage = "State must not exceed more than 20 characters")]
        public string State { get; set; }

        [MaxLength(50, ErrorMessage = " LandMark must not exceed more than 50 characters")]
        public string? LandMark { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Please enter a valid 6-digit postal code ")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is Required")]
        [MaxLength(20, ErrorMessage = "Country must not exceed more than 20 characters")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Phone number is Required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10 digit Phone Number")]
        public string Phone { get; set; }
    }
}
