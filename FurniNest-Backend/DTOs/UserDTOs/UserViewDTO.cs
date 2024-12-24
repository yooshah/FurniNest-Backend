namespace FurniNest_Backend.DTOs.AdminDTO
{
    public class UserViewDTO
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool AccountStatus { get; set; }

        public  DateTime CreatedAt { get; set; }


    }
}
