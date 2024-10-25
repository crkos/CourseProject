namespace CourseProject.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Template> Templates { get; set; }
    }
}
