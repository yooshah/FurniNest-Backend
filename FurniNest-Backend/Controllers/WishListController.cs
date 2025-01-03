using FurniNest_Backend.Models;
using FurniNest_Backend.Services.WishListService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniNest_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        public WishListController(IWishListService wishListService)
        { 
            _wishListService = wishListService;
        }


        [HttpPost("AddToWishList")]
        [Authorize(Roles ="user")]
        public async Task<IActionResult> AddToWishList([FromQuery] int productId)
        {

            try
            {


                if (HttpContext.Items["UserId"] == null || !int.TryParse(HttpContext.Items["UserId"].ToString(), out int userId))
                {
                    return Unauthorized(new ApiResponse<string>(401, "Unauthorized userId !,Adding item to WishList failed"));
                }

                var res = await _wishListService.AddToWishList(userId, productId);

                if (res.StatusCode == 401)
                {
                    return Unauthorized(new ApiResponse<string>(401, "Unauthorized userId !,Adding item to WishList failed"));
                }

                if (res.StatusCode == 200)
                {
                    return Ok(res);
                }

                return BadRequest(new ApiResponse<string>(400, "Bad request", null, res.Message));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal server Error", ex.Message));
            }


        }

        [HttpDelete("RemoveFromWishList")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> RemoveFromWishList(int productId)
        {
            try
            {
               
                var userCheck = int.TryParse(HttpContext.Items["UserId"].ToString(), out int userId);
                if (!userCheck)
                {
                    return Unauthorized(new ApiResponse<string>(401, "Invalid User, authentication failed"));
                }

               
                var result = await _wishListService.RemoveFromWishList(userId, productId);

             
                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }

                if (result.StatusCode == 404)
                {
                    return NotFound(result);
                }

                return BadRequest("Bad Request");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("viewWishListItems")]

        [Authorize(Roles ="user")]

        public async Task<IActionResult> GetUserWishList()
        {
            try
            {
                var userCheck = int.TryParse(HttpContext.Items["UserId"].ToString(), out int userId);

                if (!userCheck)
                {

                    return Unauthorized(new ApiResponse<string>(401, "Invalid User, authentication failed"));
                }
                var res = await _wishListService.GetWishListProducts(userId);

                if (res.StatusCode == 401)
                {
                    return Unauthorized(new ApiResponse<string>(401, "Invalid User, authentication failed"));
                }

                if (res.StatusCode == 200)
                {
                    return Ok(res);
                }
                return BadRequest(new ApiResponse<string>(400, "Bad Request!"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Internal Server Error! ,{ex.Message}");
            }

        }



    }
}
