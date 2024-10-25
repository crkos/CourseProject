using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject.Models
{
    public class Form
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [ForeignKey("TemplateId")]
        public virtual Template Template { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
