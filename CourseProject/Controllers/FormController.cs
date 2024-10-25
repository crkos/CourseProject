using CourseProject.Data;
using CourseProject.Interfaces;
using CourseProject.Models;
using CourseProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Controllers
{
    [Authorize]
    [Route("form")]
    public class FormController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IFormService _formService;

        public FormController(AppDBContext appDBContext, UserManager<User> userManager, IFormService formService)
        {
            _dbContext = appDBContext;
            _userManager = userManager;
            _formService = formService;
        }


        [HttpGet("{id}/fill")]
        public async Task<IActionResult> Fill(int id)
        {
            var template = _dbContext.Templates
                .Include(t => t.Questions)
                .OrderByDescending(t => t.Questions.Max(q => q.Order))
                .FirstOrDefault(t => t.Id == id);

            if (template == null)
            {
                return NotFound("Template not found");
            }

            var user = await _userManager.GetUserAsync(User);

            if (template.CreatedBy != user.Id)
            {
                return RedirectToAction("view", new { id = id });
            }

            return View(new IndividualTemplateView
            {
                Template = template,
                Liked = false
            });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromForm] FormFill model)
        {
            await _formService.Fill(model, await _userManager.GetUserAsync(User));


            return Ok();
        }
    }
}
