﻿
using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductDTO> AddProduct(AddProductDTO newProduct,IFormFile image);
        Task <ProductDTO> GetProductById(int id);
        Task <ProductDTO>UpdateProduct(int id,AddProductDTO updateProduct,IFormFile image);

        Task<bool> DeleteProductById(int id);

        Task<List<ProductDTO>> GetAllProducts();

        Task<List<ProductDTO>> GetProductByPagination(int pagenumber = 1, int pageSize = 5);

        Task<ApiResponse<List<ProductDTO>>> GetroductByCategory(int categoryId);

        Task<List<ProductDTO>> SearchProduct(string searchText);

        


    }
}
