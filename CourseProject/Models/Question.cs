using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }  // e.g., 'text', 'multiple choice', 'multi line', 'int'
        public bool IsVisible { get; set; }
        public int Order { get; set; }

        [ForeignKey("TemplateId")]
        public virtual Template Template { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
