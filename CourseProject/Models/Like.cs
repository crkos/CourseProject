using CourseProject.CustomAtributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int LikedBy { get; set; }
        public int? TemplateId { get; set; }
        public int? AnswerId { get; set; }
        public int? CommentId { get; set; }
        public DateTime LikedDate { get; set; }

        [ForeignKey("TemplateId")]
        public virtual Template Template { get; set; }

        [ForeignKey("AnswerId")]
        public virtual Answer Answer { get; set; }

        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }

        [ForeignKey("LikedBy")]
        [OwnerId]
        public virtual User User { get; set; }
    }
}
