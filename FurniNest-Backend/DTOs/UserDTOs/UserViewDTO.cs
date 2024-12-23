namespace FurniNest_Backend.DTOs.AdminDTO
{
    public class UserViewDTO
    {

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool AccountStatus { get; set; }

        public  DateTime CreatedAt { get; set; }


    }
}
