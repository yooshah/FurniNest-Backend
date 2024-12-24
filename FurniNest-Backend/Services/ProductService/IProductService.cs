
using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.ProductService
{
    public interface IProductService
    {
        Task<int> AddProduct(AddProductDTO newProduct);
        Task <ProductDTO> GetProductById(int id);
        Task <bool>UpdateProduct(int id,AddProductDTO updateProduct);

        Task<bool> DeleteProductById(int id);

        Task<List<ProductDTO>> GetAllProducts();

        Task<List<ProductDTO>> GetProductByPagination(int pagenumber = 1, int pageSize = 5);

        Task<ApiResponse<List<ProductDTO>>> GetroductByCategory(int categoryId);

        Task<List<ProductDTO>> SearchProduct(string searchText);


    }
}
