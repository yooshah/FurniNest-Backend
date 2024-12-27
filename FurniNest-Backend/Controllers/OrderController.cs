using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Models;
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
        [HttpGet("AdminViewUserOrder")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> GetuserOrderByAdmin(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("Invalid User Id");
                }
                var res= await _orderService.GetUserOrderByAdmin(userId);
                return Ok(new ApiResponse<List<AdminViewOrderDTO>>(200,"Successfully fetched user order",res));

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error! {ex.Message}");
            }

        }

        [HttpPatch("ChangeOrderStatus")]
        //[Authorize(Roles = "admin")]

        public async Task<IActionResult>ChangeOrderStatus(int orderId, string orderStatus)
        {
            try
            {
                var orderStatusRes = await _orderService.ChangeOrderStatus(orderId, orderStatus);

                if (orderStatusRes)
                {
                    return Ok(new ApiResponse<string>(200, $"Succefully update OrderStatus ,OrderId-{orderId} ", orderStatus));
                }
                return Ok(new ApiResponse<string>(200, $"Failed to Update OrderStatus of OrderId-{orderId},",$"Opertaion-{orderStatus}"));

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error! {ex.Message}");
            }

        }

        [HttpGet("TotalRevenue")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            try
            {
                var res = await _orderService.TotalRevenue();

                return Ok(new ApiResponse<decimal>(200, "Successfully Fetch Total Revenue ", res));


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal Sever error ${ex.Message}");
            }

        }
        [HttpGet("TotalProductSold")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetTotalProductCount()
        {
            try
            {
                var res = await _orderService.TotalProductSold();

                return Ok(new ApiResponse<decimal>(200, "Successfully Fetch Total Product sold ", res));


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal Sever error ${ex.Message}");
            }


        }
    }
}
