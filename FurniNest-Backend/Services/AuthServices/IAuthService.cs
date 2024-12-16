using FurniNest_Backend.DTOs.UserDTOs;

namespace FurniNest_Backend.Services.AuthServices
{
    public interface IAuthService
    {

        Task<bool> Register(UserRegisterDTO userRegister);

        Task<UserResonseDTO> Login(UserLoginDTO userLogin );
    }
}
