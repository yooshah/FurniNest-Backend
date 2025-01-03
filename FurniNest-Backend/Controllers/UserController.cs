﻿using CloudinaryDotNet.Actions;
using FurniNest_Backend.DTOs.AdminDTO;
using FurniNest_Backend.Models;
using FurniNest_Backend.Services.AdminService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniNest_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("Admin/ChangeUserAccountStatus")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> ToggleAccountStatus(int userId)
        {

            try
            {
                var res = await _userService.ChangeUserAccountStatus(userId);

                if (res.StatusCode == 404)
                {
                    return NotFound(res);
                }
                if (res.StatusCode == 200)
                {
                    return Ok(res);
                }
                return BadRequest("Bad Request,Change User AccountStatus failed");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server,{ex.Message}");
            }
        }

        [HttpGet("GetAllUserDetails")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> GetAllUserDetails()
        {
            try
            {
                var res = await _userService.ViewAllUser();

                return Ok(res);


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server,{ex.Message}");
            }
        }

        [HttpGet("ViewUserById")]

        //[Authorize(Roles = "admin")]

        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new ApiResponse<string>(400, "InvalidUserID"));
                }

                var res = await _userService.ViewUserById(userId);

                if (res == null)
                {
                    return NotFound(new ApiResponse<string>(404, "User Not Found"));
                }

                return Ok(new ApiResponse<UserViewDTO>(200, $"Successfully fetched user with Id-{userId}",res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server,{ex.Message}");
            }
        }


    }
}
