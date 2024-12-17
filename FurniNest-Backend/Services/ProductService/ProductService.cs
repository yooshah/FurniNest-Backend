using AutoMapper;
using CloudinaryDotNet;
using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;
using FurniNest_Backend.Services.CloudinaryService;
using Microsoft.EntityFrameworkCore;

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

            if (categoryCheck == null) {
                throw new InvalidOperationException("Category with this Id doesn't exist");

                
            }
            try
            {
                var imgUrl = await _cloudinaryService.UploadProductImage(image);
                var AddingProduct = _mapper.Map<Product>(newProduct);
                AddingProduct.Image = imgUrl;
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
    }
}
