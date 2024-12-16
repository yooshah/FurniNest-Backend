using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.JwtServices
{
    public interface IJwtService
    {

        string? TokenGenerator(User user);
    }
}
