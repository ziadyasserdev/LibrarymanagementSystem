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
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext dbContext;
        public BookRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> CountNumOfBooksOfCategory(int categoryId)
        {
            return await dbContext.Books.CountAsync(x => x.CategoryId == categoryId);
        }

      

      

        public bool IsBookTitleUnique(string title, int authorId)
        {
            return !dbContext.Books.Any(x =>
                          x.AuthorId == authorId &&
                          x.Title.ToLower() == title.ToLower());
        }

        public bool IsBookTitleUniqueExclusive(string title, int bookId, int authorId)
        {


            return !dbContext.Books.Any(c => c.Title.ToLower() == title.ToLower() && c.AuthorId == authorId && c.Id != bookId);
        }

        public bool IsISBNUnique(string isbn)
        {
            return !dbContext.Books.Any(c => c.ISBN == isbn);
        }

        public bool IsISBNUniqueExclusive(string isbn, int id)
        {
            return !dbContext.Books.Any(c => c.ISBN == isbn && c.Id != id);
        }
    }
}
