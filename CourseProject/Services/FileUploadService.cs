using CourseProject.Interfaces;

namespace CourseProject.Services
{
    public class FileUploadService
    {
        private readonly IUploader _uploader;

        public FileUploadService(IUploader uploader)
        {
            _uploader = uploader;
        }

        public Uri ProccessAndUploadFile(Stream fileStream, string? fileName)
        {
            return _uploader.UploadFile(fileStream, fileName);
        }

        public string TransformImage(Uri imageUrl, int width, int height)
        {
            return _uploader.TransformImage(imageUrl, width, height);
        }
    }
}
