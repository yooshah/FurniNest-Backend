using FurniNest_Backend.Models;
using FurniNest_Backend.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniNest_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("AddToCart/{productId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

             


                var res = await _cartService.AddToCart(userId, productId);

                if (res.StatusCode == 200)
                {
                    return Ok(res);
                }
                if (res.StatusCode == 208)
                {
                    return StatusCode(208, res);
                }
                if (res.StatusCode == 404)
                {
                    return NotFound(res);
                }

                return BadRequest(new ApiResponse<string>(400, "Bad request!!", null, res.Message));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal server Error", ex.Message));

            }
        }

        [HttpGet("User/ViewCart")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> ViewCartByUser()
        {
            // var userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            if (HttpContext.Items["UserId"] == null || !int.TryParse(HttpContext.Items["UserId"].ToString(), out int userId))
            {
                return Unauthorized(new ApiResponse<string>(401, "Invalid User, Adding to Cart Failed"));
            }


            var res = await _cartService.ViewCartByUser(userId);

            try
            {

                if (res.StatusCode == 200)
                {
                    return Ok(res);
                }
             

                if (res.StatusCode == 401)
                {
                    return Unauthorized();
                }
                if (res.StatusCode == 404)
                {
                    return NotFound(res);
                }

                return BadRequest(new ApiResponse<string>(400, "Bad request", null, res.Message));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal server Error", ex.Message));
            }
        }

        [HttpDelete("DeleteCartItems/{deleteCartItemId}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> DeleteCartItem(int deleteCartItemId)
        {
            Console.WriteLine($"Trying to delete item with ID: {deleteCartItemId}");
            var userCheck = int.TryParse(HttpContext.Items["UserId"].ToString(), out int userId);

            if (!userCheck)
            {
                return Unauthorized(new ApiResponse<string>(401, "Invalid User,user Authentication Failed"));
            }

            try
            {

               
                var res = await _cartService.DeleteCartByUser(userId, deleteCartItemId);

                if (res.StatusCode == 200)
                {
                    return Ok(res);
                }

               
                if (res.StatusCode == 401)
                {
                    return Unauthorized(res);
                }
                if (res.StatusCode == 404)
                {
                    return NotFound(res);
                }
                return BadRequest("Bad Request");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error, {ex.Message}");
            }
        }

        [HttpPatch("CartQuantity/{cartItemId}/increment")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> IncreaseCartQuantity(int cartItemId)
        {

            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

            try
            {

                var res=await _cartService.IncreaseCartItemQuantity(userId, cartItemId);

                if (res.StatusCode == 404)
                {
                    return NotFound(res);
                }

                if(res.StatusCode == 409)
                {
                    return Conflict(res);
                }
                
                if(res.StatusCode == 200)
                {
                    return Ok(res);
                }
                return BadRequest(res);


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server Error {ex.Message}");

            }

        }

        [HttpPatch("CartQuantity/{cartItemId}/decrement")]
        [Authorize(Roles ="user")]
        public async Task<IActionResult> DecreaseQuantity(int cartItemId)
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            try
            {
                var res=await _cartService.DecresaeQuantity(userId, cartItemId);

                if (!res)
                {
                    return NotFound(new ApiResponse<bool>(404,"Not Found!",res));
                }

                return Ok(res);


            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server Error {ex.Message}");
            }
        }

       
    }
}
