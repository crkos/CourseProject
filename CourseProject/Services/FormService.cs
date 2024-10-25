using CourseProject.Data;
using CourseProject.Interfaces;
using CourseProject.Models;
using CourseProject.ViewModels;

namespace CourseProject.Services
{
    public class FormService : IFormService
    {
        private readonly AppDBContext _dbContext;

        public FormService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Fill(FormFill form, User user)
        {
            var answers = form.Questions.Select(a => new Answer
            {
                QuestionId = a.QuestionId,
                AnswerText = a.QuestionResponse,
                SubmittedBy = user.Id,
                SubmittedDate = DateTime.Now,
            }).ToList();


            var newForm = new Form
            {
                TemplateId = form.TemplateId,
                CreatedBy = user.Id,
                CreatedDate = DateTime.Now,
                Answers = answers
            };

           
            _dbContext.Forms.Add(newForm);
            await _dbContext.SaveChangesAsync();
        }
    }
}
