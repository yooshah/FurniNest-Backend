using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Models;
using FurniNest_Backend.Services.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniNest_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {

        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService) 
        {
            _addressService = addressService;
        }

        [HttpPost("CreateOrderAddress")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> CreateAddress([FromBody] OrderAddressDTO createAddress)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _addressService.CreateShippingAddress(userId, createAddress);

                if (res.StatusCode == 400)
                {
                    return BadRequest("Bad Request,Address is null ");
                }

                if(res.StatusCode == 422)
                {
                    return StatusCode(422, "3 addresses limit reached!");
                }
                return Ok(res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }



        }
        [HttpGet("GetDeliveryAddress")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> GetDeliceryAddress()
        {

            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _addressService.GetShippingAddress(userId);

                if (res.StatusCode == 401)
                {
                    return Unauthorized(new ApiResponse<string>(401, "Invalid User,Fetching ShippingAddress Failed"));
                }

                return Ok(res);


            }
            catch (Exception ex)
            {
                return StatusCode(500, $" Internal Server Error,{ex.Message}");
            }

        }

        [HttpDelete("RemoveShippingAddress/{addressId}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> RemoveshippingAddressbyUser(int addressId)
        {
            var userId= Convert.ToInt32(HttpContext.Items["UserId"]);

            try
            {

                if (addressId <= 0)
                {
                    return BadRequest(new ApiResponse<string>(400, "Invalid Address is ,Removing Shipping Address failed"));
                }

                var res=await _addressService.RemoveShippingAddressByUser(userId, addressId);
                if (!res)
                {
                    return NotFound(new ApiResponse<string>(404, "Address Not Found ,Removing Shipping Address failed"));
                }
                return Ok(new ApiResponse<bool>(200,"Successfully deleted Shipping Address",res));

            }
            catch (Exception ex)
            {
                return StatusCode(500, $" Internal Server Error,{ex.Message}");
            }


        }
    }
}
