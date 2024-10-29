using CourseProject.Models;
using CourseProject.ViewModels;

namespace CourseProject.Interfaces
{
    public interface ITemplateService
    {
        public Task Create(Template template);
        public Task Update(TemplateVM template);
        public Task Delete(int templateId);
        public Task<Template?> GetTemplateAsync(int id);
        public Task<bool> HasUserLikedTemplate(User? user, Template? template);
        public Task Like(User user, Template template);
        public Task RemoveLikeAsync(User user, Template template);
        public Task<int> GetMaxOrder(int templateId);
    }
}
