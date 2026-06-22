using LibrarymanagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Contracts.Repositories
{
    public interface IAuthorRepository:IGenericRepository<Author>
    {
        public bool IsAuthorNameExists(string name);
        public bool IsAuthorNameExistsExclusive(string name,int id);
        public Task<Author> GetAuthorByName(string name);
        public Task<int> GetAuthorIdByName(string name);
    }
}
