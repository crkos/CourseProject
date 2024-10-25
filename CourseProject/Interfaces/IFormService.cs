using CourseProject.Models;
using CourseProject.ViewModels;

namespace CourseProject.Interfaces
{
    public interface IFormService
    {
        public Task Fill(FormFill form, User user);
    }
}
