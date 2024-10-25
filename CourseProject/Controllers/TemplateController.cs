using CourseProject.Data;
using CourseProject.Hubs;
using CourseProject.Interfaces;
using CourseProject.Models;
using CourseProject.Services;
using CourseProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Controllers
{
    [Route("template")]
    public class TemplateController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly ITemplateService _templateService;
        private readonly UserManager<User> _userManager;
        private readonly FileUploadService _fileUploadService;
        private readonly IHubContext<CommentsHub> _hubContext;

        public TemplateController(AppDBContext dbContext, ITemplateService templateService, UserManager<User> userManager, FileUploadService fileUploadService, IHubContext<CommentsHub> hubContext) {
            _dbContext = dbContext;
            _templateService = templateService;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
            _hubContext = hubContext;
        }

        [HttpGet("{id}/edit")]
        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            var template = _dbContext.Templates
                .Include(t => t.Questions)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .OrderByDescending(t => t.Questions.Max(q => q.Order))
                .FirstOrDefault(t => t.Id == id);

            if (template == null)
            {
                return NotFound("Template not found");
            }

            var user = await _userManager.GetUserAsync(User);
            var liked = await _templateService.HasUserLikedTemplate(user, template);

            if (template.CreatedBy != user.Id)
            {
                return RedirectToAction("view", new { id = id });
            }

            return View(new IndividualTemplateView
            {
                Template = template,
                Liked = liked
            });
        }

        [HttpGet("{id}/view")]
        public async Task<IActionResult> View(int id)
        {
            var template = _dbContext.Templates
                .Include(t => t.Likes)
                .Include(t => t.Questions)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .OrderByDescending(t => t.Questions.Max(q => q.Order))
                .FirstOrDefault(t => t.Id == id);
            var user = await _userManager.GetUserAsync(User);
            var alreadyLiked = await _templateService.HasUserLikedTemplate(user, template);

            if (template == null)
            {
                return NotFound("Template not found");
            }

            return View(new IndividualTemplateView
            {
                Template = template,
                Liked = alreadyLiked
            });
        }

        [Authorize]
        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikeTemplate(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var template = await _templateService.GetTemplateAsync(id);

            if (template == null)
            {
                return NotFound("Template not found.");
            }

            if (await _templateService.HasUserLikedTemplate(user, template))
            {
                await _templateService.RemoveLikeAsync(user, template);
                return Ok(new
                {
                    message = "You've disliked this template!",
                    liked = false
                });
            }

            await _templateService.Like(user, template);
            return Ok(new
            {
                message = "You've liked this template!",
                liked = true
            });
        }


        [Authorize]
        [HttpPost("Clone")]
        public async Task<IActionResult> Clone([FromBody] int templateId)
        {
            var defaultTemplate = _dbContext.Templates.Include(t => t.Questions)
                                                      .FirstOrDefault(t => t.Id == templateId);
            var user = await _userManager.GetUserAsync(User);

            // Clone the template (create a new Template object)
            var newTemplate = new Template
            {
                Name = "New Form",
                Description = defaultTemplate.Description,
                CreatedBy = user.Id,
                CreatedDate = DateTime.Now,
                Questions = new List<Question>(),
                TopicId = defaultTemplate.TopicId,
                Tags = defaultTemplate.Tags
            };

            // Clone each question and assign it to the new template
            foreach (var question in defaultTemplate.Questions)
            {
                var newQuestion = new Question
                {
                    QuestionText = question.QuestionText,
                    QuestionType = question.QuestionType,
                    IsVisible = question.IsVisible,
                    Order = question.Order,
                };
                newTemplate.Questions.Add(newQuestion);
            }

            // Add the new template (and related questions) to the database
            await _templateService.Create(newTemplate);

            return Ok(Json(new
            {
                newTemplate.Id,
                newTemplate.Name
            }));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] TemplateVM model)
        {
            await _templateService.Update(model);

            return RedirectToAction("Index", new { id = model.TemplateId });
        }

        [Authorize]
        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion([FromBody] int templateId)
        {
            var template = await _templateService.GetTemplateAsync(templateId);

            if (template == null)
            {
                return NotFound("Template not found.");
            }

            var maxOrder = await GetMaxOrderForTemplate(templateId);

            var newQuestion = new Question
            {
                QuestionText = "New Question",
                QuestionType = "TEXT",
                IsVisible = true,
                Order = maxOrder + 1, 
                TemplateId = template.Id
            };

            await _dbContext.AddAsync(newQuestion);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Id = newQuestion.Id,
                QuestionText = newQuestion.QuestionText,
                QuestionType = newQuestion.QuestionType,
                IsVisible = newQuestion.IsVisible,
                Order = newQuestion.Order
            });
        }

        [Authorize]
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(CommentVM model)
        {
            var user = await _userManager.GetUserAsync(User);

            // Create and save the comment
            var comment = new Comment
            {
                CommentText = model.CommentText,
                TemplateId = model.TemplateId,
                CommentedBy = user.Id,
                CreatedDate = DateTime.Now
            };
            await _dbContext.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveComment", model.TemplateId, comment.CommentText, user.UserName, comment.CreatedDate.ToString("MMMM dd, yyyy"));

            return Ok();
        }

        [Authorize]
        [HttpPost("UploadImage")]
        public IActionResult UploadImage([FromForm] IFormFile image)
        {
            using var stream = image.OpenReadStream();

            var imageUrl = _fileUploadService.ProccessAndUploadFile(stream, image.FileName);

            return Json(new
            {
                data = new
                {
                    filePath = imageUrl
                }
            });
        }

        private async Task<int> GetMaxOrderForTemplate(int templateId)
        {
            var maxOrder = await _dbContext.Question
                .Where(q => q.TemplateId == templateId)
                .MaxAsync(q => (int?)q.Order) ?? 0; 

            return maxOrder;
        }

    }
}
