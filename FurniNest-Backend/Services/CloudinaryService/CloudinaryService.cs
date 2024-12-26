using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FurniNest_Backend.DTOs.ProductDTOs;
using System.Threading.Tasks;

namespace FurniNest_Backend.Services.CloudinaryService
{
    public class CloudinaryService:ICloudinaryService
    {

        private readonly Cloudinary _cloudinary;
       

        public CloudinaryService(IConfiguration configuration)
        {

            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];


            var account=new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ProductImgAndImgIdDTO> UploadProductImage(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                return null;
            }
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder="FurniNestImageStore",
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                   
                    throw new Exception($"Cloudinary upload error: {uploadResult.Error.Message}");
                }

                var imgRes = new ProductImgAndImgIdDTO
                {
                    ImgId = uploadResult.PublicId,
                    ImgUrl= uploadResult.SecureUrl?.ToString()
                    

                };
               
                return imgRes;
            }
        }
        public async Task<bool> DeleteProductImage(string imageId)
        {
            if (string.IsNullOrEmpty(imageId))
            {
                return false; // No image to delete
            }

            var deletionParams = new DeletionParams(imageId);

            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.Result == "ok")
            {
                return true; // Successfully deleted
            }
            else
            {
                throw new Exception($"Cloudinary deletion error: {deletionResult.Error?.Message}");
            }
        }





    }
}
