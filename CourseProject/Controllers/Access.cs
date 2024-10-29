using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseProject.ViewModels;
using CourseProject.Models;
using CourseProject.Data;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Controllers
{
    public class Access : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public Access(AppDBContext AppDbContext, SignInManager<User> signInManager, UserManager<User> userManager) 
        {
            _dbContext = AppDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserVM model)
        {
            if (model.Password.Length == 0 || model.ConfirmPassword.Length == 0)
            {
                ViewData["Message"] = "Password should not be empty. Please enter at least 1 character.";
                return View();
            }

            if (model.Password != model.ConfirmPassword)
            {
                ViewData["Message"] = "Passwords do not match.";
                return View();
            }

            // Create a new user object
            User user = new User()
            {
                UserName = model.Name, // Or model.Name depending on your requirement
                Name = model.Name,
                LastName = model.LastName,
                Email = model.Email,
                NormalizedEmail = model.Email,
                Status = "Active",
                PhoneNumber = "6121112016"
            };

            // Use UserManager to create the user and hash the password automatically
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Optionally, sign the user in after successful registration
                var userSync = await _signInManager.CreateUserPrincipalAsync(user);
  
                await _signInManager.SignInAsync(user, false);
              
                return RedirectToAction("Index", "Home");
            }

            // If registration fails, display errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {

            User? user = await _dbContext.Users
                                  .Where(u =>
                                      u.Email == model.Email
                                  ).FirstOrDefaultAsync();

            if (user == null)
            {
                ViewData["Message"] = "The email/password is incorrect";
                return View();
            }

            if (user.Status == "Blocked")
            {
                ViewData["Message"] = "This user is currently blocked, contact an administrator to unblock you.";
                return View();
            }

            user.LastLogin = DateTime.Now;

            try
            {
                _dbContext.SaveChanges();
            } catch (DbUpdateException ex)
            {
                ViewData["Message"] = "Something unexpected happened, try again later.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                return View();
            }
        }
    }
}
