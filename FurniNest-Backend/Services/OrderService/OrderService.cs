using AutoMapper;
using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Enums;
using FurniNest_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

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



            // decimal totalAmount = userOrder.CartItems
            //.Where(item => item.Product != null)
            // .Sum(item => item.Product.Price * item.Quantity);



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

        public async Task<ApiResponse<OrderViewDTO>> GetOrderItems(int userId)
        {

            var userOrder= await _context.Orders.Include(x=>x.OrderItems).ThenInclude(p=>p.Product).FirstOrDefaultAsync(x=>x.UserId==userId);

            var orderaddress = await _context.ShippingAddresses.FirstOrDefaultAsync(x=>x.Id==userOrder.ShippingAddressId);

            if (userOrder == null || userOrder.OrderItems == null || !userOrder.OrderItems.Any())
            {
                return new ApiResponse<OrderViewDTO>(200, "Order List is Empty"); // Return empty list if no orders found
            }

            var resOrder=userOrder.OrderItems.Select(item=>new OrderItemDTO
            {
                ProductName=item.Product.Name,
                Quantity=item.Quantity,
                Price=item.Product.Price,
                TotalPrice=(item.Product.Price * item.Quantity)

            }).ToList();


            var totalPay=userOrder.OrderItems.Sum(x=>x.Quantity*x.Price);

            var res = new OrderViewDTO
            {
                TransactionId=userOrder.TransactionId,
                TotalAmount=totalPay,
                OrderStatus=Convert.ToString(userOrder.OrderStatus),
                DeliveryAdrress=$"{orderaddress.Address},{orderaddress.City},{orderaddress.State} {orderaddress.Country},{orderaddress.PostalCode}",
                Phone=orderaddress.Phone,
                OrderDate=userOrder.OrderDate,
                Items=resOrder,
               

            };


            return new  ApiResponse<OrderViewDTO>(200, "Successfully Fetched User Cart", res);


        }

        public async Task<ApiResponse<string>> CreateShippingAddress(int userUId, OrderAddressDTO orderAddressDTO)
        {
            try
            {
                if (orderAddressDTO == null)
                {
                    return new  ApiResponse<string>(400, "Bad Request,Address is null");


                }

                
                // A user can have atmost 3 address

                var addressCount = await _context.ShippingAddresses.CountAsync(x => x.UserId == userUId);

                if(addressCount >= 3)
                {
                    return new  ApiResponse<string>(400, "User cannot have more than 3 addresses.");
                }

                

                var newAddress=_mapper.Map<ShippingAddress>(orderAddressDTO);
                newAddress.UserId= userUId;
                await _context.ShippingAddresses.AddAsync(newAddress);
                await _context.SaveChangesAsync();

                
                return  new  ApiResponse<string>(200, "Successfully Created Shipping Address");
            }
            catch (Exception ex) { 
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse<List<OrderAddressDTO>>> GetShippingAddress(int userId)
        {

            var userAddress = await _context.Users.Include(x => x.ShippingAddresses).FirstOrDefaultAsync(x => x.Id == userId);
            
            if(userAddress == null)
            {
                return new ApiResponse<List<OrderAddressDTO>>(401, "Invalid User");
            }

            var addressList = userAddress.ShippingAddresses.Count();

            if (addressList <= 0)
            {
                return new ApiResponse<List<OrderAddressDTO>>(200, "Invalid User",new List<OrderAddressDTO>());
            }

            var res=new List<OrderAddressDTO>();

            foreach (var address in userAddress.ShippingAddresses)
            {
                var showAddress=_mapper.Map<OrderAddressDTO>(address);
                res.Add(showAddress);
            }

            return new ApiResponse<List<OrderAddressDTO>>(200, "Shipping Address Fetched Successfully", res);


        }



      


    }
}
