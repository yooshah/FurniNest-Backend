using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.CartDTOs;
using FurniNest_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniNest_Backend.Services.CartService
{
    public class CartService:ICartService
    {
        private readonly AppDbContext _context;
        public CartService(AppDbContext context) 
        { 
            _context = context;
        }
        public async Task<ApiResponse<string>> AddToCart(int userId, int productId)
        {
            var user = await _context.Users
                                     .Include(u => u.Cart)
                                     .ThenInclude(c => c.CartItems) 
                                     .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new ApiResponse<string>(401, "Invalid User, Adding to Cart Failed");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                return new ApiResponse<string>(404, "Invalid Product ID, Adding to Cart Failed");
            }

            if (user.Cart == null)
            {
                user.Cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                await _context.Carts.AddAsync(user.Cart);
                await _context.SaveChangesAsync();
            }

            var existingCartItem = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingCartItem == null)
            {
                var cartItem = new CartItem
                {
                    CartId = user.Cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(200, "Product Added to Cart successfully!");
            }
            else
            {
                existingCartItem.Quantity++;
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(200, "Product Quantity Increased in Cart");
            }
        }

        public async Task<ApiResponse<List<CartItemViewDTO>>> ViewCartByUser(int userId)
        {

           
                var userCheck = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (userCheck == null)
                {

                    return new ApiResponse<List<CartItemViewDTO>>(401, "Unauthorized: Invalid User! ");

                }


                var userCart = await _context.Users
                    .Include(x => x.Cart)
                    .ThenInclude(x => x.CartItems)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(c => c.Category)
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (userCart?.Cart?.CartItems == null || !userCart.Cart.CartItems.Any())
                {
                    return new ApiResponse<List<CartItemViewDTO>>(200, "Cart is Fetched", new List<CartItemViewDTO>());
                }

                var allUserCartItem = userCart.Cart.CartItems.Select(item => new CartItemViewDTO
                {
                    Name = item.Product?.Name,
                    Brand = item.Product?.Brand,
                    Category = item.Product?.Category?.Name,
                    Image = item.Product?.Image,
                    Rating = item.Product?.Rating ?? 0,
                    Price = item.Product?.Price ?? 0,
                    Quantity = item.Quantity,
                    TotalPrice = (Convert.ToInt32(item.Product?.Price ?? 0) * (item.Quantity))

                }).ToList();
                return new ApiResponse<List<CartItemViewDTO>>(200, "Successfull fetched Cart Items!", allUserCartItem);

            }


        public async Task<ApiResponse<string>> DeleteCartByUser(int userId, int CartItemId)
        {



            var userCartItems = await _context.Users
                .Include(x => x.Cart)
                .ThenInclude(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.Id == userId);


            if (userCartItems == null)
            {
                return new ApiResponse<string>(401, "User not Exist!");
            }



            if (userCartItems.Cart?.CartItems == null)
            {
                return new ApiResponse<string>(404, "Deleting Cart Item Failed, No such item exists in the cart.");
            }

            var deleteItem = userCartItems.Cart.CartItems.FirstOrDefault(x => x.Id == CartItemId);

            if (deleteItem == null)
            {
                return new ApiResponse<string>(404, "deleting Cart Item Failed,No such Item exist on Cart");
            }

            userCartItems.Cart.CartItems.Remove(deleteItem);

            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, $"Deleted Cart Item Id-{CartItemId}");


        }

    }
}
