using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FurniNest_Backend.Services.WishListService
{
    public class WishListService : IWishListService
    {

        private readonly AppDbContext _context;
        public WishListService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> AddToWishList(int userId, int productId)
        {

            var userWishList = await _context.Users.Include(x => x.WishList).ThenInclude(y => y.WishListItems).FirstOrDefaultAsync(x => x.Id == userId);

            if (userWishList == null)
            {
                return new ApiResponse<string>(401, "Invalid User ,Adding to wishlist failed");

            }

            var productExists = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (productExists==null)
            {
                return new ApiResponse<string>(404, "Product not found");
            }



            if (userWishList.WishList == null)
            {
                var createWishlist = new WishList
                {
                    userId = userWishList.Id,
                    WishListItems = new List<WishListItem>()

                };

                await _context.WishLists.AddAsync(createWishlist);
                await _context.SaveChangesAsync();
            }

            var productInWishList = userWishList.WishList.WishListItems.FirstOrDefault(x => x.ProductId == productId);

            if (productInWishList != null)
            {
                return new ApiResponse<string>(200, "product Already Exist in WishList!");

            }
            var wishItem = new WishListItem
            {
                WishListId = userWishList.WishList.Id,
                ProductId = productId

            };
            await _context.WishListItems.AddAsync(wishItem);

            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, $"Product with product Id-{productId} Succcessfully Added to WishList!");


        }

        public async Task<ApiResponse<string>> RemoveFromWishList(int userId, int productId)
        {
            
            var userWishList = await _context.Users.Include(x => x.WishList)
                                                    .ThenInclude(y => y.WishListItems)
                                                    .FirstOrDefaultAsync(x => x.Id == userId);

            if (userWishList == null || userWishList.WishList == null)
            {
                return new ApiResponse<string>(404, "WishList not found");
            }

           
            var productInWishList = userWishList.WishList.WishListItems
                                               .FirstOrDefault(x => x.ProductId == productId);

            if (productInWishList == null)
            {
                return new ApiResponse<string>(404, "Product not found in wish list");
            }

            _context.WishListItems.Remove(productInWishList);

            
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, $"Product with Id {productId} successfully removed from WishList");
        }

        public async Task<ApiResponse<List<ProductDTO>>> GetWishListProducts(int userId)
        {

            var userWishList = await _context.Users
                                    .Include(x => x.WishList)
                                    .ThenInclude(w => w.WishListItems)
                                    .ThenInclude(w => w.Product) 
                                    .ThenInclude(x=>x.Category)
                                    .FirstOrDefaultAsync(x => x.Id == userId);

            if (userWishList == null)
            {
                return new ApiResponse<List<ProductDTO>>(401, "User not found or unauthorized", null);
            }

            if (userWishList.WishList == null )
            {
                return new ApiResponse<List<ProductDTO>>(200, "Wishlist not found, returning empty wishlist", new List<ProductDTO>());
            }
            var wishListProducts = userWishList.WishList.WishListItems
                                            .Select(wi => new ProductDTO
                                            {
                                                ProductId = wi.ProductId,
                                                Name = wi.Product.Name,
                                                Category = wi.Product.Category.Name,
                                                Rating = wi.Product.Rating,
                                                Image = wi.Product.Image,
                                                Brand = wi.Product.Brand,
                                                Price = wi.Product.Price,
                                                Stock = wi.Product.Stock,

                                            }).ToList();

            return new ApiResponse<List<ProductDTO>>(200,"Successfully Loaded all WishList Items",wishListProducts);
                                            


        }
    }
}
