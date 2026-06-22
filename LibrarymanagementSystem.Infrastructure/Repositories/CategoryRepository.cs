using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Models;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<Category> GetCategoryByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetCategoryIdByName(string name)
        {
            return await dbContext.Categories.Where(x=>x.Name.ToLower()==name.ToLower()).Select(x=>x.CategoryId).FirstOrDefaultAsync();
        }

        public bool IsCategoryNameExists(string name)
        {
            return dbContext.Categories.Any(c => c.Name == name);
        }

        public bool IsCategoryNameExistsExclusive(string name, int id)
        {
            return dbContext.Categories.Any(c => c.Name == name&&c.CategoryId!=id);
        }
    }
}
