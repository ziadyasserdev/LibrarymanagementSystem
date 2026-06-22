using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Repositories
{
    public class LoanBookRepository : GenericRepository<LoanBook>, ILoanBookRepository
    {
        private readonly ApplicationDbContext dbContext;
        public LoanBookRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> GetAllActiveLoanAsync(string userId)
        {
            return await dbContext.LoanBooks.Where(x => x.UserId == userId && x.Status == LoanStatus.Active).CountAsync();
        }
        public async Task<LoanBook> GetByLoaner(int bookId, string userId)
        {
            return await dbContext.LoanBooks
     .Where(x => x.BookCopyId == bookId &&
            x.UserId == userId
            && x.Status != LoanStatus.Closed
           )
     .OrderByDescending(x => x.LoanDate)
      .FirstOrDefaultAsync();

        }

        //public async Task<LoanBook> GetByLoaner(int bookId, string userId)
        //{
        //    return await dbContext.LoanBooks.Include(c => c.Fine)

        //         .Where(x => x.BookCopyId == bookId && x.Status == LoanStatus.Active && x.UserId == userId).FirstOrDefaultAsync();
        //}

        public async Task<List<LoanBook>> GetLateBooks()
        {
            return await dbContext.LoanBooks
                .Include(x => x.BookCopy)
                .ThenInclude(c => c.Book)
                .Include(c => c.User)
                .Where(c => c.ReturnDate == null && c.DueDate < DateTime.Now)
                .ToListAsync();
        }

        public async Task<List<LoanBook>> GetLoanBooksByUserId(string userId)
        {
            return await dbContext.LoanBooks
                .Include(c => c.BookCopy)
                .Include(c => c.User)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<LoanBook>> GetREturnBooks()
        {
            return await dbContext.LoanBooks
                .Include(x => x.BookCopy)
                .ThenInclude(x => x.Book)
                .Include(x => x.User)
               .Where(x => x.ReturnDate != null)
               .ToListAsync();

        }

        public async Task<bool> HasActiveLoanForBookAsync(string userId, int bookId)
        {
            return await dbContext.LoanBooks.AnyAsync(c => c.UserId == userId && c.BookCopyId == bookId && c.ReturnDate == null);
        }
        //LoanStatus
        public async Task<bool> HasFines(string userId)
        {
            return await dbContext.LoanBooks
                .AnyAsync(c => c.UserId == userId && (c.Status == LoanStatus.PendingFinePayment || c.Status == LoanStatus.Lost));
        }

        public async Task<bool> HasOverdueBooksAsync(string userId)
        {
            return await dbContext.LoanBooks.AnyAsync(c => c.UserId == userId && c.ReturnDate == null && c.DueDate < DateTime.Now &&
            c.Status != LoanStatus.Lost);

        }
    }
}
