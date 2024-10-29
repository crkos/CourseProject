namespace CourseProject.ViewModels
{
    public class TemplateVM
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public IFormFile? TemplateImage { get; set; }
        public List<UpdateQuestionViewModel>? Questions { get; set; }
    }

    public class UpdateQuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string? IsVisible { get; set; }
        public string QuestionType { get; set; }
    }
}
