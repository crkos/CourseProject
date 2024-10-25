using CourseProject.Interfaces;
using CourseProject.Models;

namespace CourseProject.Services
{
    public class SearchService
    {
        private readonly ISearch<Template> _templateSearch;

        public SearchService(ISearch<Template> templateSearch)
        {
            _templateSearch = templateSearch;
        }

        public IEnumerable<Template> PerformSearch(string term)
        {
            return _templateSearch.Search(term);
        }
    }
}
