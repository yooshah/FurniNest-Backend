
using FurniNest_Backend.DTOs.ProductDTOs;

namespace FurniNest_Backend.Services.ProductService
{
    public interface IProductService
    {
        Task<int> AddProduct(AddProductDTO newProduct, IFormFile file);
    }
}
