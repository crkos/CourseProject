using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject.Models
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public int TopicId { get; set; }
        public Uri? Image { get; set; }
        public List<string>? Tags { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
