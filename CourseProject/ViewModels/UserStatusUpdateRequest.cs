namespace CourseProject.ViewModels
{
    public class UserStatusUpdateRequest
    {
        public List<int> UserIds { get; set; }
        public string Status { get; set; }
    }
}
