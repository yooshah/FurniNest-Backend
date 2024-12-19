
using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.ProductService
{
    public interface IProductService
    {
        Task<int> AddProduct(AddProductDTO newProduct, IFormFile file);
        Task <ProductDTO> GetProductById(int id);
        Task <bool>UpdateProduct(int id,AddProductDTO updateProduct, IFormFile image=null);

        Task<List<ProductDTO>> GetAllProducts();

    }
}
