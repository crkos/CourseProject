using Microsoft.AspNetCore.Mvc;
using CourseProject.Services;
using CourseProject.ViewModels;

namespace CourseProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Index(string search)
        {
            var viewModel = new SearchTemplatesVM();

            if (!string.IsNullOrEmpty(search))
            {
                var results = _searchService.PerformSearch(search).ToList();
                viewModel.Templates = results;
            }

            return View(viewModel);
        }
    }
}
