using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public int SubmittedBy { get; set; }
        public DateTime SubmittedDate { get; set; }


        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        [ForeignKey("SubmittedBy")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
    }
}
