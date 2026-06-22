using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Settings;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.BackgroundJobs.Fines
{

    public class FineJobs : IFineJobs
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;
        private readonly LibraryPolicySetting options;

        public FineJobs(IUnitOfWork unitOfWork, IEmailService emailService, IOptions<LibraryPolicySetting> options)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            this.options = options.Value;
        }

 
        public async Task CalculateFinesForOverdueBooks()
        {
            var overdueBooks = unitOfWork.LoanBooks.Query()
                .Where(x => x.DueDate < DateTime.Now && x.ReturnDate == null);

            foreach (var loanBook in overdueBooks)
            {
                var minutesLate = Math.Max(0, (DateTime.UtcNow - loanBook.DueDate).TotalMinutes);
                if (minutesLate > 0)
                {
                    bool fineExists = await unitOfWork.Fines.Query()
                        .AnyAsync(f => f.LoanBookId == loanBook.Id && f.FineType == FineType.LateReturn);

                    if (!fineExists)
                    {
                        var fine = new Fine
                        {
                            LoanBookId = loanBook.Id,
                            TotalAmount = (decimal)minutesLate * options.FinePerDay,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "System",
                            FineType = FineType.LateReturn,
                            DueDate = DateTime.Now.AddMinutes(2)
                        };
                        await unitOfWork.Fines.AddAsync(fine);
                    }

                    await unitOfWork.SaveAsync();
                }
            }
        }


        public async Task CalculateFineForReturnedBook(int loanBookId, string userEmail, string userName, string bookTitle, DateTime dueDate)
        {
            var loanBook = await unitOfWork.LoanBooks.GetByIdAsync(loanBookId);
            if (loanBook == null) return;

            var minutesLate = Math.Max(0, (DateTime.Now - dueDate).TotalMinutes);
            string message;

            if (minutesLate > 0)
            {
                bool fineExists = await unitOfWork.Fines.Query()
                    .AnyAsync(f => f.LoanBookId == loanBook.Id && f.FineType == FineType.LateReturn);

                if (!fineExists)
                {
                    var fine = new Fine
                    {
                        LoanBookId = loanBook.Id,
                        TotalAmount = (decimal)minutesLate * options.FinePerDay,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System",
                        FineType = FineType.LateReturn,
                        DueDate = DateTime.UtcNow.AddMinutes(2)
                    };

                    await unitOfWork.Fines.AddAsync(fine);
                    await unitOfWork.SaveAsync();
                }

                var fineDueDate = DateTime.UtcNow.AddMinutes(2);
                message = $"The book was returned {Math.Ceiling(minutesLate)} minutes late. A fine has been applied. Please pay by {fineDueDate:MMMM dd, yyyy HH:mm}.";
            }
            else
            {
                message = "The book was returned on time. No fine is due.";
            }

          
            await emailService.SendEmailAsync(
                userEmail,
                "Book Return Status",
                $"Hello {userName},\n\n{message}\n\nLoaned book: '{bookTitle}'\nLibrary System"
            );
        }
    }
}
