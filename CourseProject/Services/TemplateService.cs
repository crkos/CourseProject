using CourseProject.Data;
using CourseProject.Interfaces;
using CourseProject.Models;
using CourseProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly AppDBContext _dbContext;
        private readonly FileUploadService _fileUploadService;

        public TemplateService(AppDBContext dbContext, FileUploadService fileUploadService)
        {
            _dbContext = dbContext;
            _fileUploadService = fileUploadService;
        }

        public async Task Create(Template template)
        {
            _dbContext.Templates.Add(template);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TemplateVM model)  
        {
            var template = await _dbContext.Templates.Include(t => t.Questions).FirstOrDefaultAsync(t => t.Id == model.TemplateId);

            if (template == null)
            {
                throw new Exception("This template does not exists!");
            }

            if (model.TemplateImage != null)
            {
                using var imageStream = model.TemplateImage.OpenReadStream();
                var image = _fileUploadService.ProccessAndUploadFile(imageStream, model.TemplateImage.FileName);
                template.Image = image;
            }

            template.Name = model.TemplateName;
            template.Description = model.TemplateDescription;

            if (model.Questions != null)
            {
                foreach (var questionModel in model.Questions)
                {
                    var question = template.Questions.FirstOrDefault(q => q.Id == questionModel.QuestionId);

                    if (question == null)
                    {
                        continue;
                    }
                    question.QuestionText = questionModel.QuestionText;
                    question.QuestionType = questionModel.QuestionType;
                    if (questionModel.IsVisible == "on")
                    {
                        question.IsVisible = true;
                    } else
                    {
                        question.IsVisible = false;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int templateId)
        {
            var template = await _dbContext.Templates
                .FirstOrDefaultAsync(t => t.Id == templateId);

            if (template == null)
            {
                throw new Exception("Template not found.");
            }

            _dbContext.Templates.Remove(template);

            await _dbContext.SaveChangesAsync();
        }


        public async Task<Template?> GetTemplateAsync(int id)
        {
            return await _dbContext.Templates
                .Include(t => t.Likes)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> HasUserLikedTemplate(User? user, Template? template)
        {
            if (user == null || template == null)
            {
                return false;
            }

            return await _dbContext.Likes
                .AnyAsync(l => l.TemplateId == template.Id && l.LikedBy == user.Id);
        }

        public async Task Like(User user, Template template)
        {
            var like = new Like
            {
                TemplateId = template.Id,
                LikedBy = user.Id
            };

            await _dbContext.Likes.AddAsync(like);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveLikeAsync(User user, Template template)
        {
            var existingLike = await _dbContext.Likes
                .FirstOrDefaultAsync(l => l.TemplateId == template.Id && l.LikedBy == user.Id);

            if (existingLike != null)
            {
                _dbContext.Likes.Remove(existingLike);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> GetMaxOrder(int templateId)
        {
            var maxOrder = await _dbContext.Question
                .Where(q => q.TemplateId == templateId)
                .MaxAsync(q => (int?)q.Order) ?? 0;

            return maxOrder;
        }
    }
}
