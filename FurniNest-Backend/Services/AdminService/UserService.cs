using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.AdminDTO;
using FurniNest_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniNest_Backend.Services.AdminService
{
    public class UserService:IUserService
    {
        
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        
        }

        public async Task<ApiResponse<string>> ChangeUserAccountStatus(int UserId)
        {
            var userAccount = await _context.Users.SingleOrDefaultAsync(x => x.Id == UserId);

            if (userAccount == null)
            {
                return new ApiResponse<string>(404, "User Not Found");
            }


            userAccount.AccountStatus = !userAccount.AccountStatus;

            string statusMessage = userAccount.AccountStatus ? "activated" : "blocked";

            await _context.SaveChangesAsync();
            return new ApiResponse<string>(200, $"User account has been {statusMessage} successfully.");
        }

        public async Task<List<UserViewDTO>> ViewAllUser()
        {
                var allUsers = await _context.Users.Where(x=>x.Role=="user").Select(user => new UserViewDTO
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    AccountStatus = user.AccountStatus,
                    CreatedAt = user.CreatedAt,

                }
                ).ToListAsync();

            return allUsers;
            


        }
    }
}
