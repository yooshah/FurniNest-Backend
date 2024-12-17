using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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

        public async Task<string> UploadProductImage(IFormFile file)
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
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                   
                    throw new Exception($"Cloudinary upload error: {uploadResult.Error.Message}");
                }

               
                return uploadResult.SecureUrl?.ToString();
            }
        }


    }
}
