using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;
using FurniNest_Backend.Services.CloudinaryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FurniNest_Backend.Services.ProductService
{
    public class ProductService:IProductService
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        
        public ProductService(AppDbContext context,ICloudinaryService cloudinaryService,IMapper mapper,ILogger<ProductService> logger) 
        { 
            _context = context;
            _cloudinaryService = cloudinaryService;
             _mapper = mapper;
            _logger = logger;
        }


        public async Task<int> AddProduct(AddProductDTO newProduct,IFormFile image)
        {

            if (newProduct == null) {

                throw new ArgumentNullException(nameof(newProduct), "Product cannot be null");
                
            }

            var categoryCheck = await _context.Categories.FirstOrDefaultAsync(x=>x.CategoryId==newProduct.CategoryId);

            if (categoryCheck == null ) {
                throw new InvalidOperationException("Category with this Id doesn't exist");



            }

            if (image == null) {
                throw new InvalidOperationException("Image is Not Uploaded");
            }
            var categoryExist = _context.Categories.FirstOrDefault(x => x.CategoryId == newProduct.CategoryId);
             if (categoryExist == null) throw new Exception("Category with this Id doesn't exist");

            try
            {
                var cloudRes = await _cloudinaryService.UploadProductImage(image);
                var AddingProduct = _mapper.Map<Product>(newProduct);
                AddingProduct.Image = cloudRes.ImgUrl;
                AddingProduct.CloudImgId = cloudRes.ImgId;
                await _context.Products.AddAsync(AddingProduct);
                await _context.SaveChangesAsync();
                return AddingProduct.ProductId;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {

                throw new Exception($"An Exception has been occuured while creating new product {ex.Message}");
            }


        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            try
            {
                var existingProduct = await _context.Products.Include(x=>x.Category).FirstOrDefaultAsync(p => p.ProductId == id);
                if (existingProduct == null)
                {
                    return null;

                }

                var viewProduct = _mapper.Map<ProductDTO>(existingProduct);

                viewProduct.Category = existingProduct.Category.Name;

                return viewProduct;



            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);

            }
            catch (Exception ex) {
                throw new Exception($"An Exception has been occuured while retrieving product using ID, {ex.Message}");
            }



        }

        
        public async Task<bool> UpdateProduct(int id, AddProductDTO updtProduct,IFormFile image )
        {

            try
            {
                if (updtProduct == null)
                {
                    throw new ArgumentNullException("Update Product cannot be null");
                }
                var existProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                var categoryExist = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == updtProduct.CategoryId);
                if (categoryExist == null)
                {
                    throw new KeyNotFoundException("Category with the provided ID not found, unable to modify product.");
                }


               
                

                if (existProduct != null)
                {

                    
                   
                    if(string.IsNullOrEmpty(updtProduct.Name))
                    {
                    existProduct.Name = updtProduct.Name;

                    }
                    
                    existProduct.Price = updtProduct.Price;
                    existProduct.Rating = updtProduct.Rating;
                    existProduct.CategoryId = updtProduct.CategoryId;
                    existProduct.Brand = updtProduct.Brand;
                    existProduct.Stock = updtProduct.Stock;

                    if (image != null)
                    {
                        var cloudRes = await _cloudinaryService.UploadProductImage(image);

                        await _cloudinaryService.DeleteProductImage(existProduct.CloudImgId);
                        
                        existProduct.Image = cloudRes.ImgUrl;
                        existProduct.CloudImgId= cloudRes.ImgId;

                    }







                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception($"Product with {id} not found");
                }
            }
            catch (DbException ex)
            {
               
                throw new Exception("Database error occurred: " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An exception occurred while updating the product: {ex.Message}");
            }







        }

       
        public async Task<bool> DeleteProductById(int prdtId)
        {
            
            var deleteProduct = await _context.Products.FirstOrDefaultAsync(x=>x.ProductId == prdtId);

            if(deleteProduct == null)
            {
                return false;
            }


             _context.Products.Remove(deleteProduct);

            await _cloudinaryService.DeleteProductImage(deleteProduct.CloudImgId);
            await _context.SaveChangesAsync();
            return true;


           
        }

        public async Task<List<ProductDTO>> GetAllProducts()
        {

            var products = await _context.Products.Include(x=>x.Category).ToListAsync();

            if (products.Count > 0)
            {

                var allProduct = products.Select(x =>
                new ProductDTO
                {
                    ProductId=x.ProductId,
                    Name = x.Name,
                    Price = x.Price,
                    Brand = x.Brand,
                    Image = x.Image,
                    Category = x.Category.Name,
                    Rating = x.Rating,
                    Stock = x.Stock,
                   
                   
                }
                ).ToList();
                return allProduct;
            }
            return new List<ProductDTO>();

        }

        public async Task<List<ProductDTO>> GetProductByPagination(int pagenumber = 1, int pageSize = 5)
        {

            var products=await _context.Products.Include(x=>x.Category).Skip((pagenumber - 1) * pageSize).Take(pageSize).ToListAsync();

            
            var productRes=products.Select(pdt=>new ProductDTO
            {
                ProductId=pdt.ProductId,
                Name=pdt.Name,
                Price=pdt.Price,
                Category=pdt.Category.Name,
                Brand=pdt.Brand,
                Rating=pdt.Rating,
                Image=pdt.Image,
                Stock=pdt.Stock,
                
            }).ToList();

            return productRes;




        }

        public async Task<ApiResponse<List<ProductDTO>>> GetroductByCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                return new ApiResponse<List<ProductDTO>>(400, "Invalid Category Id");
            }
            var produttCategory=await _context.Categories.Include(x=>x.Products).FirstOrDefaultAsync(x=>x.CategoryId== categoryId);
            if (produttCategory == null)
            {
                return new ApiResponse<List<ProductDTO>>(404, "Category not found");
            }

            var result = produttCategory.Products.Select(item => new ProductDTO
            {
                ProductId=item.ProductId,
                Name = item.Name,
                Price = item.Price,
                Rating = item.Rating,
                Image = item.Image,
                Category = item.Category.Name,
                Brand = item.Brand,
                Stock = item.Stock,
            }).ToList();

            return new ApiResponse<List<ProductDTO>>(200,"Successfully fetched products by category",result);

            
        }

        public async  Task<List<ProductDTO>> SearchProduct(string searchText)
        {

            if (string.IsNullOrEmpty(searchText))
            {
                return new List<ProductDTO>();
            }

            var searchProducts = await _context.Products.Include(x => x.Category).Where(x => x.Name.ToLower().Contains(searchText.ToLower())).ToListAsync();

            var resultProduct = searchProducts.Select(item => new ProductDTO
            {
                ProductId=item.ProductId,
                Name = item.Name,
                Price = item.Price,
                Rating= item.Rating,
                Image = item.Image,
                Category=item.Category.Name,
                Brand=item.Brand,
                Stock=item.Stock,


            }).ToList();

            return resultProduct;




        }







    }
}
