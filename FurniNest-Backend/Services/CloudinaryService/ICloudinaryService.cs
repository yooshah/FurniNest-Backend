namespace FurniNest_Backend.Services.CloudinaryService
{
    public interface ICloudinaryService
    {

        Task<string> UploadProductImage(IFormFile file);
    }
}
