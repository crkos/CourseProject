using CourseProject.Data;
using CourseProject.Interfaces;
using CourseProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Infraestructure
{
    public class TemplateSearch : ISearch<Template>
    {
        private readonly AppDBContext _dbContext;
        public TemplateSearch(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Template> Search(string query)
        {
            return _dbContext.Templates
                .FromSqlRaw("SELECT * FROM Templates WHERE FREETEXT((Name), {0})", query)
                .ToList();
        }
    }
}
