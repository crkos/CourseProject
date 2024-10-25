using Microsoft.AspNetCore.Identity;

namespace CourseProject.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime LastLogin { get; set; }

        public virtual ICollection<Form> Forms { get; set; }
        public virtual ICollection<Template> Templates { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }

        public string FullName()
        {
            var fullName = $"{Name} ${LastName}";
            return fullName;
        }
    }
}
