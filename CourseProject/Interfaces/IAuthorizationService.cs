namespace CourseProject.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<bool> CanEditResource<T>(T resource);
    }
}
