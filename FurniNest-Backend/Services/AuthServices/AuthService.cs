using AutoMapper;
using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.UserDTOs;
using FurniNest_Backend.Models;
using FurniNest_Backend.Services.JwtServices;
using Microsoft.EntityFrameworkCore;

namespace FurniNest_Backend.Services.AuthServices
{
    public class AuthService:IAuthService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<IAuthService> _logger;
        private readonly IJwtService _jwtService;
        public AuthService(AppDbContext context,IMapper mapper,ILogger<IAuthService> logger,IJwtService jwtService) 
        {
            _context = context;
            _mapper = mapper;   
            _jwtService = jwtService;
        }
         public async Task<bool> Register(UserRegisterDTO userRegister)
        {


            try
            {
                if (userRegister == null)
                {

                    throw new ArgumentNullException("User data cannnot be null");
                }

                var isuUerExist = await _context.Users.SingleOrDefaultAsync(u => u.Email == userRegister.Email);
                if (isuUerExist != null)
                {
                    return false;
                }
                userRegister.Password = BCrypt.Net.BCrypt.HashPassword(userRegister.Password);

                var newUser = _mapper.Map<User>(userRegister);
                _context.Add(newUser);

                await _context.SaveChangesAsync();
                return true;


            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"database error:{ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UserResonseDTO> Login(UserLoginDTO userLogin)
        {
            try
            {
                //_logger.LogInformation("User attempting to log ....");
               

                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userLogin.Email);

                if (user == null)
                {
                    //_logger.LogWarning("user not found");
                    return new UserResonseDTO { Error = "Not Found" };
                }
                //_logger.LogInformation("User found. Validating password...");

                var passwordCheck = ValidatePassword(userLogin.Password, user.Password);

                if (!passwordCheck)
                {

                    //_logger.LogWarning("invalid password");
                    return new UserResonseDTO { Error= "Invalid Password" };
                }

                if (!user.AccountStatus)
                {
                    //_logger.LogWarning("user is blocked");
                    return new UserResonseDTO { Error = "User Blocked" };
                }
                //_logger.LogInformation("generating token");

                var token = _jwtService.TokenGenerator(user);
                return new UserResonseDTO 
                { 
                  Id=user.Id,
                  Name=user.Name,
                  Email=userLogin.Email,
                  Role=user.Role,
                  Token = token
                };


            }
            catch (Exception ex) {
                _logger.LogError($"Error in login:{ex.Message}");
                throw;
            }
        }

        private bool ValidatePassword(string password, string hashPassword) {

            return BCrypt.Net.BCrypt.Verify(password, hashPassword);    
        }
    }
}
