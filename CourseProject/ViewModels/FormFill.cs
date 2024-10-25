namespace CourseProject.ViewModels
{
    public class FormFill
    {
        public int TemplateId { get; set; }
        public List<QuestionsViewModel>? Questions { get; set; }
    }

    public class QuestionsViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string QuestionResponse { get; set; }
    }
}
