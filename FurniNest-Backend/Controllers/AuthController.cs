using FurniNest_Backend.DTOs.UserDTOs;
using FurniNest_Backend.Models;
using FurniNest_Backend.Services.AuthServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace FurniNest_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        [HttpPost("Register")]

        public async Task<IActionResult> UserRegistration([FromBody] UserRegisterDTO newUser)
        {
            try
            {
                bool registerDone = await _authService.Register(newUser);

                if (!registerDone)
                {
                    return Conflict(new ApiResponse<string> ( 409,"User Already Exist"));
                }
                return Ok(new ApiResponse<bool>(200, "User Registered successfully", registerDone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin(UserLoginDTO userLogin)
        {

            try
            {
                var loginResult = await _authService.Login(userLogin);
                

                if (loginResult.Error == "Not Found")
                {
                    return NotFound(new ApiResponse<string>(404, "Not Found", null, "Please SignUp,Email not found"));
                }

                if (loginResult.Error == "Invalid Password")
                {
                    return BadRequest(new ApiResponse<string>(400, "BadRequest", null, loginResult.Error));

                }
                if (loginResult.Error == "User Blocked")
                {
                    return StatusCode(403, new ApiResponse<string>(403, "Forbidden", null, loginResult.Error));
                }
                return Ok(new ApiResponse<UserResonseDTO>(200, "Login Successfull", loginResult));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal server error occured", null, ex.Message));
            }
        }
    }
}
