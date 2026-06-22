using LibrarymanagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Contracts.Repositories
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        public bool IsCategoryNameExists(string name);
            public bool IsCategoryNameExistsExclusive(string name,int id);
            public Task<Category> GetCategoryByName(string name);
        public Task<int> GetCategoryIdByName(string name);
    }
}
