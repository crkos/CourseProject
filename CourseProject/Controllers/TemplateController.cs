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
        private readonly AuthorizationService _authorizationService;

        public TemplateController(AppDBContext dbContext, ITemplateService templateService, UserManager<User> userManager, FileUploadService fileUploadService, IHubContext<CommentsHub> hubContext, AuthorizationService authorizationService = null)
        {
            _dbContext = dbContext;
            _templateService = templateService;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
            _hubContext = hubContext;
            _authorizationService = authorizationService;
        }

        [HttpGet("{id}/edit")]
        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            var template = _dbContext.Templates
                .Include(t => t.Questions.OrderBy(q => q.Order))
                    .ThenInclude(t => t.Answers)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Include(t => t.Forms)
                .FirstOrDefault(t => t.Id == id);

            if (template == null)
            {
                return NotFound("Template not found");
            }


            if (!await _authorizationService.CanEditResource(template))
            {
                return Unauthorized();
            }

            var user = await _userManager.GetUserAsync(User);
            var liked = await _templateService.HasUserLikedTemplate(user, template);
            var answerChartData = template.Questions.Select(q => new AnswerChartData
            {
                QuestionText = q.QuestionText,
                AnswerCounts = q.Answers
            .GroupBy(a => a.AnswerText)
            .ToDictionary(g => g.Key, g => g.Count())
            }).ToList();

            return View(new EditTemplateVM
            {
                Template = template,
                Liked = liked,
                AnswerCharData = answerChartData
            });
        }

        [HttpGet("{id}/view")]
        public async Task<IActionResult> View(int id)
        {
            // Maybe i can do something on the service?
            var template = _dbContext.Templates
                .Include(t => t.Likes)
                .Include(t => t.Questions.OrderBy(q => q.Order))
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
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
            if (!await _authorizationService.CanEditResource(await _templateService.GetTemplateAsync(model.TemplateId)))
            {
                return Unauthorized();
            }

            await _templateService.Update(model);

            return RedirectToAction("Index", new { id = model.TemplateId });
        }

        [Authorize]
        [HttpPatch("order")]
        public async Task<IActionResult> Order([FromBody] OrderVM model)
        {
            var questionToMove = await _dbContext.Question.FirstOrDefaultAsync(t => t.Id == model.QuestionId);

            if (questionToMove == null)
            {
                return NotFound("Question not found");
            }

            if (await _authorizationService.CanEditResource(questionToMove))
            {
                return Unauthorized();
            }

            var questionAtNewOrder = await _dbContext.Question
                .FirstOrDefaultAsync(q => q.Order == model.NewOrder && q.TemplateId == model.TemplateId);

            if (questionAtNewOrder == null)
            {
                return NotFound("No question found with the specified new order");
            }

            questionAtNewOrder.Order = model.OldOrder;
            questionToMove.Order = model.NewOrder;

            await _dbContext.SaveChangesAsync();

            return Ok("Order updated successfully");
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

            if (await _authorizationService.CanEditResource(template))
            {
                return Unauthorized();
            }

            var maxOrder = await _templateService.GetMaxOrder(templateId);

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
        [HttpDelete]
        public async Task<IActionResult> Delete(int templateId)
        {
            if (await _authorizationService.CanEditResource(await _templateService.GetTemplateAsync(templateId)))
            {
                return Unauthorized();
            }


            await _templateService.Delete(templateId);

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
    }
}
