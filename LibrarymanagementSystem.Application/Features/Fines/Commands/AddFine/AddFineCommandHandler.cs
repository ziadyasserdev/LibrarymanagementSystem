using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Settings;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Commands.AddFine
{
    public class AddFineCommandHandler : IRequestHandler<AddFineCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;
        private readonly ICurrentUserService currentUserService;
        private readonly FineSettings options;

        public AddFineCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService,ICurrentUserService currentUserService,IOptions<FineSettings> options)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            this.currentUserService = currentUserService;
            this.options = options.Value;
        }

        public async Task<Result<int>> Handle(AddFineCommand request, CancellationToken cancellationToken)
        {
            var loanBook = await unitOfWork.LoanBooks.Query()
        .Include(l => l.User)
        .FirstOrDefaultAsync(l => l.Id == request.LoanBookId);



            if (loanBook == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Loan book not found.");

            var bookCopy = await unitOfWork.BookCopies.Query()
                  .FirstOrDefaultAsync(x => x.Id == loanBook.BookCopyId && !x.IsDeleted);
            if(bookCopy == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Book copy not found or deleted");
            var alreadyPaidSameFine = await unitOfWork.Fines.Query()
  .AnyAsync(x =>
      x.LoanBookId == request.LoanBookId &&
      x.FineType == request.FineType &&
      x.PaidAmount >= x.TotalAmount);

            if (alreadyPaidSameFine)

                return Result<int>.Failure(ResultStatus.Conflict, "This Fine already paid and same type");







            if (request.FineType == FineType.LostBook)
            {
                if (loanBook.ReturnDate.HasValue)
                    return Result<int>.Failure(
                      ResultStatus.Conflict,
                      "This book already returned"
                  );

                if (DateTime.Now < loanBook.DueDate)
                    return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "The due date has not passed yet. You cannot mark this book as lost before its return date."
                );
                bookCopy.IsLost=true;
                bookCopy.Status=BookCopyStatus.Lost;
                
            }




            if (request.FineType == FineType.LateReturn)
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Late return fines are generated automatically by the system."
                );


            var hasUnpaidLostFine = await unitOfWork.Fines.Query()
                .AnyAsync(f =>
                    f.LoanBookId == loanBook.Id &&
                    f.FineType == FineType.LostBook &&
                    f.PaidAmount < f.TotalAmount,
                    cancellationToken);

            if (hasUnpaidLostFine)
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A lost book fine already exists and must be fully paid first."
                );


            if (request.FineType == FineType.DamagedBook)
            {
                if (!loanBook.ReturnDate.HasValue)
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        "Damage fine can only be applied after the book is returned."
                    );


                var checkFine = await unitOfWork.Fines.Query()
                    .AnyAsync(x => x.LoanBookId == loanBook.Id &&
                    x.FineType == FineType.LostBook);

                if (checkFine)
                    return Result<int>.Failure(
                   ResultStatus.Conflict,
                   "This book already lost, you can't add damage fine of it"
               );


                var damageFineExists = await unitOfWork.Fines.Query()
                    .AnyAsync(f =>
                        f.LoanBookId == loanBook.Id &&
                        f.FineType == FineType.DamagedBook,
                        cancellationToken);

                if (damageFineExists)
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        "A damage fine already exists for this loan book."
                    );

                bookCopy.Status=BookCopyStatus.Damaged;
                bookCopy.IsDamaged=true;
            }




            if (request.FineType != FineType.Manual)
            {
                var unpaidSameTypeFineExists = await unitOfWork.Fines.Query()
                    .AnyAsync(f =>
                        f.LoanBookId == loanBook.Id &&
                        f.FineType == request.FineType &&
                        f.PaidAmount < f.TotalAmount,
                        cancellationToken);

                if (unpaidSameTypeFineExists)
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        "An unpaid fine of the same type already exists."
                    );
            }

            var fine = new Fine
            {
                LoanBookId = loanBook.Id,
                FineType = request.FineType,
                TotalAmount = request.TotalAmount,
                PaidAmount = 0,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.UserName!,
                Notes = request.Notes ?? request.FineType switch
                {
                    FineType.DamagedBook => "Damage fine applied due to book damage.",
                    FineType.LostBook => "Lost book fine applied.",
                    FineType.Manual => "Manual fine applied by administrator.",
                    _ => "Fine applied."
                },
                DueDate = DateTime.UtcNow.AddMinutes(options.PaymentGracePeriodDays)
            };

            if (request.FineType == FineType.LostBook)
                loanBook.Status = LoanStatus.Lost;
            else if (loanBook.Status != LoanStatus.Lost)
                loanBook.Status = LoanStatus.PendingFinePayment;



            await unitOfWork.Fines.AddAsync(fine);
            await unitOfWork.SaveAsync();


            if (!string.IsNullOrWhiteSpace(loanBook.User?.Email))
            {
                await emailService.SendEmailAsync(
                    loanBook.User.Email,
                    "Library Fine Notification",
                    $"Dear {loanBook.User.UserName},<br/>" +
                    $"A new fine has been added to your account.<br/>" +
                    $"Type: {fine.FineType}<br/>" +
                    $"Amount: {fine.TotalAmount:C}<br/>" +
                    $"Notes: {fine.Notes}"
                );
            }

            return Result<int>.Success(fine.Id);
        }
    }
}
