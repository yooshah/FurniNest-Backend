using AutoMapper;
using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.AdminDTO;
using FurniNest_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniNest_Backend.Services.AdminService
{
    public class UserService:IUserService
    {
        
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper=mapper;
        
        }

        public async Task<ApiResponse<string>> ChangeUserAccountStatus(int UserId)
        {
            var userAccount = await _context.Users.Where(x=>x.Role=="user").SingleOrDefaultAsync(x => x.Id == UserId);

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
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    AccountStatus = user.AccountStatus,
                    CreatedAt = user.CreatedAt,

                }
                ).ToListAsync();

            return allUsers;
            


        }

        public async Task<UserViewDTO> ViewUserById(int UserId)
        {

            var userRes=await _context.Users.Where(x=>x.Role=="user").FirstOrDefaultAsync(x=>x.Id == UserId);

            if(userRes == null)
            {
                return null;
            }

            var res=_mapper.Map<UserViewDTO>(userRes);
            

            return res;
        }
    }
}
