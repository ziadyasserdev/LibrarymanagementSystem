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
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AuthorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Author> GetAuthorByName(string name)
        {
           return await dbContext.Authors.FirstOrDefaultAsync(c => c.Name == name);
        }

        public Task<int> GetAuthorIdByName(string name)
        {
            return dbContext.Authors.Where(c => c.Name.ToLower() == name.ToLower()).Select(c => c.AuthorId).FirstOrDefaultAsync();
        }

        public bool IsAuthorNameExists(string name)
        {
            return  dbContext.Authors.Any(c => c.Name == name);
     
        }

        public bool IsAuthorNameExistsExclusive(string name,int id)
        {
            return dbContext.Authors.Any(c => c.Name == name&&c.AuthorId!=id);
        }
    }
}
