using LibrarymanagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Contracts.Repositories
{
    public interface ILoanBookRepository: IGenericRepository<LoanBook>
    {
        public Task<int> GetAllActiveLoanAsync(string userId);
        public Task<bool> HasOverdueBooksAsync(string userId);
        Task<bool> HasActiveLoanForBookAsync(string userId, int bookId);
        public Task<LoanBook> GetByLoaner(int bookId,string userId);
        public Task<List<LoanBook>> GetREturnBooks();
        public Task<List<LoanBook>> GetLoanBooksByUserId(string userId);
        public Task<List<LoanBook>> GetLateBooks();
        public Task<bool> HasFines(string userId);

    }
}
