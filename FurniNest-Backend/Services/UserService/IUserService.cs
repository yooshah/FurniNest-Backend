using FurniNest_Backend.DTOs.AdminDTO;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.AdminService
{
    public interface IUserService
    {
        Task<ApiResponse<string>> ChangeUserAccountStatus(int UserId);

        Task<List<UserViewDTO>> ViewAllUser();

        Task<UserViewDTO> ViewUserById(int UserId);
    }
}
