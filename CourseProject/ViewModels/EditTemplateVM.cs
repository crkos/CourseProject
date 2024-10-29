using CourseProject.Models;

namespace CourseProject.ViewModels
{
    public class EditTemplateVM
    {
        public Template Template { get; set; }
        public List<AnswerChartData> AnswerCharData { get; set; }
        public bool Liked { get; set; }
    }

    public class AnswerChartData
    {
        public string QuestionText { get; set; }
        public Dictionary<string, int> AnswerCounts { get; set; }
    }
}
