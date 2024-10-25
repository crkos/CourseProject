namespace CourseProject.Interfaces
{
    public interface IUploader
    {
        Uri UploadFile(Stream fileStream, string? fileName);
        string TransformImage(Uri imageUrl);
    }
}
