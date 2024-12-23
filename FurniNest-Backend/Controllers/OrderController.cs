using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniNest_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

        }

        [HttpPost("Razorpay/Order-Create")]
        public async Task<IActionResult> CreateRazorpayOrder(long price)
        {
            try
            {
                if (price <= 0)

                {
                    return BadRequest("Invalid Price");
                }

                var orderId =await _orderService.CreateRazorpayOrder(price);
                return Ok(orderId);

            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("PlaceOrder")]
        [Authorize(Roles ="user")]
        public async Task<IActionResult> PlaceOrder(CreateOrderDTO createOrder)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _orderService.CreateOrder(userId, createOrder);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("UserOrderDetails")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UserGetOrderItems()
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _orderService.GetOrderItems(userId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error! {ex.Message}");
            }

        }

        [HttpPost("CreateOrderAddress")]
        [Authorize(Roles ="user")]

        public async Task<IActionResult> CreateAddress(OrderAddressDTO createAddress)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _orderService.CreateShippingAddress(userId, createAddress);

                if (res.StatusCode == 400)
                {
                    return BadRequest("Bad Request,Address is nul or Already 3 Addresses of delivery Exist,Update it ");
                }
                return Ok(res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }

            

        }


        
    }
}
