using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Settings;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Commands.BorrowBooks
{
    public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IEmailService emailService;
        private readonly LibraryPolicySetting libraryPolicySetting;
        public BorrowBookCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IOptions<LibraryPolicySetting> options, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.emailService = emailService;
            this.libraryPolicySetting = options.Value;
        }
        public async Task<Result<int>> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {

            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "User is not authenticated.");

            if (currentUserService.IsDeleted)
                return Result<int>.Failure(ResultStatus.Forbidden, "Your account is deleted.");

         
            var userId = currentUserService.UserId;
            var bookCopy = await unitOfWork.BookCopies.Query()
              .Include(x => x.Book)
              .FirstOrDefaultAsync(x => x.Id == request.BookCopy && !x.IsDeleted, cancellationToken);

            if (bookCopy == null)
                return Result<int>.Failure(ResultStatus.NotFound, "BookCopy not found or deleted");



            var hasOverdueBooks = await unitOfWork.LoanBooks.Query()
                .Include(x => x.User)
                .AnyAsync(x =>
                 
                    x.User.Id == userId &&
                    x.Status == LoanStatus.Active &&
                  
                    x.DueDate < DateTime.UtcNow,
                    cancellationToken);
           
            if (hasOverdueBooks)
                return Result<int>.Failure(ResultStatus.Conflict, "You can't borrow because you have overdue books");

            var alreadyBorrowed = await unitOfWork.LoanBooks.Query()
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.BookCopyId == request.BookCopy &&
                    x.Status == LoanStatus.Active,
                    cancellationToken);

            if (alreadyBorrowed)
                return Result<int>.Failure(ResultStatus.Conflict, "You already borrowed this book.");


            var activeLoansCount = await unitOfWork.LoanBooks.GetAllActiveLoanAsync(userId);

            if (activeLoansCount >= libraryPolicySetting.MaxActiveLoansPerUser)
                return Result<int>.Failure(ResultStatus.Conflict,
                    $"User has reached the maximum limit of {libraryPolicySetting.MaxActiveLoansPerUser} active loans.");

         
            var hasUnpaidFines = await unitOfWork.Fines.Query()
                .AnyAsync(f =>
                    f.LoanBook.UserId == userId &&
                    f.PaidAmount < f.TotalAmount,
                    cancellationToken);

            if (hasUnpaidFines)
                return Result<int>.Failure(ResultStatus.Conflict, "User can't borrow new books until all fines are paid.");

          
          
            if (bookCopy.IsLost)
                return Result<int>.Failure(ResultStatus.NotFound, "This book copy is marked as lost and cannot be borrowed.");

            if (bookCopy.Status == BookCopyStatus.Loaned)
                return Result<int>.Failure(ResultStatus.Conflict, "This book copy is currently loaned out and cannot be borrowed.");

            if (bookCopy.IsDamaged)
                return Result<int>.Failure(ResultStatus.Conflict, "This book copy is damaged and cannot be borrowed.");

            if (bookCopy.Status != BookCopyStatus.Available)
                return Result<int>.Failure(ResultStatus.Failure, "Book copy is not available for borrowing");

        
          

          
            bookCopy.Status = BookCopyStatus.Loaned;

            var loanBook = new LoanBook
            {
                BookCopyId = request.BookCopy,
                UserId = userId,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddMinutes(2),
                // DueDate = DateTime.Now.AddDays(libraryPolicySetting.MaxLoanDays)
            };

            await unitOfWork.LoanBooks.AddAsync(loanBook);
            await unitOfWork.SaveAsync();

         
            var emailBody =
                $"Hello {currentUserService.UserName},<br/><br/>" +
                $"You have successfully borrowed the book: <b>{bookCopy.Book.Title}</b>.<br/>" +
                $"Borrow Date: {loanBook.LoanDate:dd MMM yyyy}<br/>" +
                $"Due Date: {loanBook.DueDate:dd MMM yyyy}<br/><br/>" +
                $"Please return the book on time to avoid any fines.<br/><br/>" +
                $"Happy reading! ";

            await emailService.SendEmailAsync(
                currentUserService.Email!,
                "Library Borrowing Confirmation",
                emailBody
            );
            return Result<int>.Success(loanBook.Id);
        }
    }
}
  