using LibrarymanagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Contracts.Repositories
{
    public interface IBookRepository:IGenericRepository<Book>
    {
   
        public bool IsISBNUnique(string isbn);
            public bool IsISBNUniqueExclusive(string isbn,int id);
        public bool IsBookTitleUnique(string title,int authorId);
            public bool IsBookTitleUniqueExclusive(string title,int bookId,int authorId);
    }
}
