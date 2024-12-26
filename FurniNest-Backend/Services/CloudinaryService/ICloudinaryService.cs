using FurniNest_Backend.DTOs.ProductDTOs;

namespace FurniNest_Backend.Services.CloudinaryService
{
    public interface ICloudinaryService
    {

        Task<ProductImgAndImgIdDTO> UploadProductImage(IFormFile file);

        Task<bool> DeleteProductImage(string imageId);
    }
}
