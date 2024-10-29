using Microsoft.EntityFrameworkCore;
using CourseProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Data
{
    public class AppDBContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Topic> Topics { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.Name).HasMaxLength(150);
                tb.Property(col => col.Email).HasMaxLength(150);
                tb.HasIndex(col => col.Email).IsUnique();
                tb.Property(col => col.Status);
                tb.Property(col => col.LastLogin);
            });

            modelBuilder.Entity<Template>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.Name).HasMaxLength(150);
                tb.Property(col => col.Description).HasMaxLength(250);
                tb.Property(col => col.CreatedBy);
                tb.Property(col => col.CreatedDate);
            });

            modelBuilder.Entity<Form>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.TemplateId);
                tb.Property(col => col.CreatedBy);
                tb.Property(col => col.CreatedDate);
            });

            modelBuilder.Entity<Answer>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.QuestionId);
                tb.Property(col => col.AnswerText);
                tb.Property(col => col.SubmittedBy);
                tb.Property(col => col.SubmittedDate);
            });

            modelBuilder.Entity<Question>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.TemplateId);
                tb.Property(col => col.QuestionText);
                tb.Property(col => col.QuestionType);
                tb.Property(col => col.Order);
            });

            modelBuilder.Entity<Comment>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.CommentText);
                tb.Property(col => col.AnswerId);
                tb.Property(col => col.CommentedBy);
                tb.Property(col => col.CreatedDate);
            });

            modelBuilder.Entity<Like>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.LikedBy);
                tb.Property(col => col.TemplateId);
                tb.Property(col => col.AnswerId);
                tb.Property(col => col.CommentId);
                tb.Property(col => col.LikedDate);
            });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Forms)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Form>()
                .HasOne(f => f.Template)
                .WithMany(t => t.Forms)
                .HasForeignKey(f => f.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId);
             

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Template>().ToTable("Templates");
            modelBuilder.Entity<Form>().ToTable("Forms");
            modelBuilder.Entity<Answer>().ToTable("Answers");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Like>().ToTable("Likes");

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "First", Status = "Active", UserName = "First", Email = "first@app.com", NormalizedEmail = "FIRST@APP.COM", PhoneNumber = "6121112016", NormalizedUserName = "FIRSTADMIN", LastName = "FIRSTADMIN", PasswordHash = "AQAAAAIAAYagAAAAEPZnSUh3qOA7/y+hlHyh8MVZuY3FHKgDKsWWhU40GI9K6ecLaIMv5ZaGX14SxcqrFA==", SecurityStamp = "7RBRNCP4BAJFPO76ZJGICDH4RYVJ24I6", ConcurrencyStamp = "3775d934-d3f1-415e-a930-14445a1b1ef6" }
                );

            modelBuilder.Entity<Topic>().HasData(
                new Topic { Id= 1, Name = "Education"}
                );

            modelBuilder.Entity<Template>().HasData(
                new Template { Id = 1, Name = "Default", Description = "Default template", CreatedBy = 1, TopicId = 1, Image = null, Tags = null, CreatedDate = DateTime.Now}
                );

            modelBuilder.Entity<Question>().HasData(
                new Question { Id = 1, QuestionText = "Question", QuestionType = "TEXT", TemplateId = 1, IsVisible = true, Order = 1}
                );

            modelBuilder.Entity<Comment>().HasData(
                new Comment { Id = 1, CommentedBy = 1, CreatedDate = DateTime.Now, CommentText = "Hello, this is a sample comment!", TemplateId = 1 }
                );
        }
    }
}
