namespace FurniNest_Backend.DTOs.UserDTOs
{
    public class UserResonseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Error { get; set; }

        public bool AccountStatus { get; set; }
    }
}
