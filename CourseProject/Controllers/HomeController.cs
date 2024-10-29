using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CourseProject.Models;
using Microsoft.AspNetCore.Authorization;
using CourseProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CourseProject.ViewModels;
using CourseProject.Services;


namespace CourseProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _dbContext;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly FileUploadService _fileUploadService;

        public HomeController(ILogger<HomeController> logger, AppDBContext dbContext, SignInManager<User> signInManager, UserManager<User> userManager, FileUploadService fileUploadService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var templates = await _dbContext.Templates.Where(c => c.Id == 1).Take(4).ToListAsync();
            var currentUserTemplates = await _dbContext.Templates.Where(c => c.CreatedBy == user.Id).ToListAsync();

            foreach (var template in templates)
            {
                /*
                 * Should i just put in the model and use it in the HTML?
                 @Model.Cloudinary.Api.UrlImgUp.Transform(
                  new Transformation().Width("auto").Dpr("auto").IsResponsive(True).Crop("scale").Angle(20))
                  .BuildImageTag("sample.jpg");
                 */

                if (template.Image != null)
                {
                    template.Image = new Uri(_fileUploadService.TransformImage(template.Image, 250, 350));
                }
            }

            foreach (var template in currentUserTemplates)
            {
                if (template.Image != null)
                {
                    template.Image = new Uri(_fileUploadService.TransformImage(template.Image, 250, 350));
                }
            }

            var viewModel = new TemplatesView
            {
                Templates = templates,
                CurrentUserTemplates = currentUserTemplates,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Access");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
