
using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.ProductService
{
    public interface IProductService
    {
        Task<int> AddProduct(AddProductDTO newProduct);
        Task <ProductDTO> GetProductById(int id);
        Task <bool>UpdateProduct(int id,AddProductDTO updateProduct);

        Task<List<ProductDTO>> GetAllProducts();

        Task<List<ProductDTO>> SearchProduct(string searchText);

    }
}
