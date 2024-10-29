using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CourseProject.Interfaces;

namespace CourseProject.Infraestructure
{
    public class CloudinaryUploader : IUploader
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryUploader()
        {
            var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
            if (string.IsNullOrEmpty(cloudinaryUrl))
            {
                throw new InvalidOperationException("Cloudinary URL not set in enviroment variables");
            }

            _cloudinary = new Cloudinary(cloudinaryUrl);
            _cloudinary.Api.Secure = true;
        }

        public Uri UploadFile(Stream fileStream, string? fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream)
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl;
        }

        public string TransformImage(Uri imageUrl, int width = 250, int height = 350)
        {
            return _cloudinary.Api.UrlImgUp.Transform(
                new Transformation().Width(width).Height(height).Crop("fill"))
                .BuildUrl(imageUrl.ToString().Split("/").Last());
        }
    }
}
