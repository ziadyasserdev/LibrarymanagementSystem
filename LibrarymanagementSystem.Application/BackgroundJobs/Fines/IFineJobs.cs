using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.BackgroundJobs.Fines
{
    public interface IFineJobs
    {
        Task CalculateFinesForOverdueBooks();
        Task CalculateFineForReturnedBook(int loanBookId, string userEmail, string userName, string bookTitle, DateTime dueDate);
    }
}
