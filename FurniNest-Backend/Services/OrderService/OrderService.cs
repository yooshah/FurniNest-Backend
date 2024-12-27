using AutoMapper;
using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Enums;
using FurniNest_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System.Reflection.Metadata.Ecma335;

namespace FurniNest_Backend.Services.OrderService
{
    public class OrderService : IOrderService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public OrderService(AppDbContext context, IConfiguration configuration,IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<string> CreateRazorpayOrder(long price)
        {

            Dictionary<string, object> razorInp = new Dictionary<string, object>();

            string transactionId = Guid.NewGuid().ToString();

            razorInp.Add("amount", Convert.ToDecimal(price) * 100);
            razorInp.Add("currency", "INR");
            razorInp.Add("receipt", transactionId);

            string key = _configuration["Razorpay:KeyId"];
            string secret = _configuration["Razorpay:KeySecret"];

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(razorInp);
            var orderId = order["id"].ToString();
            return orderId;
         }
        public async Task<bool> VerifyRazorpayPayment(RazorpayPaymentDTO payment)
        {

            try
            {
                if (payment == null || string.IsNullOrEmpty(payment.razorpay_payment_id)
                    || string.IsNullOrEmpty(payment.razorpay_orderId) || string.IsNullOrEmpty(payment.razorpay_signature))
                {
                    return false;
                }

                RazorpayClient client = new RazorpayClient(_configuration["Razorpay:KeyId"], _configuration["Razorpay:KeySecret"]);

                Dictionary<string, string> paymentVerificationDetails = new Dictionary<string, string>
                {
                    {"razorpay_payment_id", payment.razorpay_payment_id},
                    {"razorpay_orderId" ,payment.razorpay_orderId },
                    {"razorpay_signature",payment.razorpay_signature }


                };
                Utils.verifyPaymentSignature(paymentVerificationDetails);
                return true;
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }



        }

        public async Task<bool> CreateOrder(int userId, CreateOrderDTO createOrderDTO)
        {

            var userOrder = await _context.Carts.Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.UserId == userId);

            if (userOrder == null || userOrder.CartItems == null || userOrder.CartItems.Count <= 0)
            {
                return false;

            }

            foreach (var item in userOrder.CartItems)
            {
                
                var currentStock=await _context.Products.FirstOrDefaultAsync(x=>x.ProductId == item.ProductId);

                if (currentStock.Stock < item.Quantity) 
                {
                    return false;
                }

                currentStock.Stock -= item.Quantity;
                
            }



            var newOrder = new Models.Order
            {
                UserId = userId,
                TotalAmount = createOrderDTO.Totalamount,
                OrderStatus = OrderStatus.Pending,
                ShippingAddressId = createOrderDTO.AddressId,
                TransactionId = createOrderDTO.TransactionId,
                OrderItems = userOrder.CartItems.Select(item =>
                new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price,
                    TotalPrice = (item.Product.Price * item.Quantity)

                }).ToList()



            };

            
           

            await _context.Orders.AddAsync(newOrder);



            _context.Carts.Remove(userOrder);

            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<ApiResponse<List<OrderViewDTO>>> GetOrderItems(int userId)
        {


            var userOrders = await _context.Orders
      .Include(x => x.OrderItems)
      .ThenInclude(x => x.Product)
      .Where(x => x.UserId == userId)
      .ToListAsync();

            var deliveryAddresses = await _context.ShippingAddresses
                .Where(x => x.UserId == userId)
                .ToListAsync();

           
            var addressDict = deliveryAddresses
                .GroupBy(addr => addr.UserId)
                .ToDictionary(group => group.Key, group => group.FirstOrDefault()); // Pick the first address per user

           
            var orderRes = userOrders.Select(order => new OrderViewDTO
            {
                TransactionId = order.TransactionId,
                TotalAmount = order.OrderItems.Sum(x => x.Quantity * x.Price),
                DeliveryAdrress = addressDict.TryGetValue(order.UserId, out var address)
                    ? $"{address.Address}, {address.City}, {address.State} {address.Country}, {address.PostalCode}"
                    : "Address not found",
                Phone = addressDict.TryGetValue(order.UserId, out var phoneAddress) ? phoneAddress.Phone : "Phone not available",
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(orderItem => new OrderItemDTO
                {
                    ProductName = orderItem.Product.Name,
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity,
                    TotalPrice = orderItem.Price * orderItem.Quantity
                }).ToList()
            }).ToList();

            return new ApiResponse<List<OrderViewDTO>>(200, "Successfully Fetched User Orders", orderRes);

            

        }


        public async Task<List<AdminViewOrderDTO>> GetUserOrderByAdmin(int userId)
        {
            var userOrder=await _context.Users.Include(x=>x.Orders).ThenInclude(x=>x.OrderItems).FirstOrDefaultAsync(x=>x.Id==userId);

            if (userOrder == null)
            {
                return null;
            }

            var result =userOrder.Orders.Select(item=>new AdminViewOrderDTO
            {
                OrderId=item.Id,
                TransactionId=item.TransactionId,
                TotalAmount=item.TotalAmount,
                OrderStatus=item.OrderStatus.ToString(),



            }).ToList();

            return result;

        }

        public async Task<bool> ChangeOrderStatus(int orderId,string orderStatus)
        {

            var orderRes=await _context.Orders.FirstOrDefaultAsync(x=>x.Id == orderId);

            if (orderRes == null || string.IsNullOrWhiteSpace(orderStatus))
            {
                return false;
            }



            if (Enum.TryParse<OrderStatus>( orderStatus, true, out var status))
            {
                orderRes.OrderStatus = status;

                await _context.SaveChangesAsync();

                return true;

            }

            return false;


        }

       public async Task<decimal> TotalRevenue()
        {
                
            var revenue = await _context.OrdersItems.SumAsync(x => x.TotalPrice);

            return revenue;
            
            

        }

        public async Task<int> TotalProductSold()
        {

            var TotalProduct = await _context.OrdersItems.SumAsync(x => x.Quantity);

            return TotalProduct;



        }







    }
}
