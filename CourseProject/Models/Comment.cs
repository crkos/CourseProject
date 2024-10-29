using CourseProject.CustomAtributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int? TemplateId { get; set; }
        public int? AnswerId { get; set; }
        public int CommentedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("TemplateId")]
        public virtual Template Template { get; set; }

        [ForeignKey("AnswerId")]
        public virtual Answer Answer { get; set; }

        [ForeignKey("CommentedBy")]
        [OwnerId]
        public virtual User User { get; set; }
    }
}
