using Hangfire;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reservations.Commands.PromoteReservation;
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

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Commands.ReturnBooks
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, Result<int>>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IEmailService emailService;
        private readonly FineSettings options1;
        private readonly LibraryPolicySetting libraryPolicySetting;

        public ReturnBookCommandHandler(IMediator mediator,
            IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IOptions<LibraryPolicySetting> options, IEmailService emailService,IOptions<FineSettings> options1)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.emailService = emailService;
            this.options1 = options1.Value;
            this.libraryPolicySetting = options.Value;
        }
        public async Task<Result<int>> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            string message;
            var userId = currentUserService.UserId;
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "User is not authenticated.");

            var bookCopy = await unitOfWork.BookCopies.Query()
                .Include(x => x.Book)
                .FirstOrDefaultAsync(x => x.Id == request.BookCopyId && !x.IsDeleted, cancellationToken);
            if (bookCopy == null)
                return Result<int>.Failure(ResultStatus.NotFound, "BookCopy not found.");

            var loanBook = await unitOfWork.LoanBooks.GetByLoaner(request.BookCopyId, userId!);
            if (loanBook == null)
                return Result<int>.Failure(ResultStatus.Conflict, "You don't have an active loan for this book.");

            if (loanBook.ReturnDate != null)
                return Result<int>.Failure(ResultStatus.Conflict, "This book has already been returned.");


            if (loanBook.Status == LoanStatus.Lost)
            {
                var lostFine = await unitOfWork.Fines.Query()
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync(f => f.LoanBookId == loanBook.Id &&
                                              f.FineType == FineType.LostBook);





                if (lostFine != null && lostFine.IsPaid)
                {
                    bookCopy.IsLost = true;
                    bookCopy.Status = BookCopyStatus.Lost;
                    loanBook.Status = LoanStatus.Lost;
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        "This book copy is marked as lost and cannot be returned.");
                }

                if (lostFine != null && !lostFine.IsPaid)
                {

                    unitOfWork.Fines.Delete(lostFine);
                    bookCopy.IsLost = false;
                    bookCopy.Status = BookCopyStatus.Available;
                    loanBook.ReturnDate = DateTime.Now;



                    var MinutesLate = Math.Max(0, (DateTime.Now - loanBook.DueDate).TotalMinutes);
                    if (MinutesLate > 0)
                    {
                        bool fineExists = await unitOfWork.Fines.Query()
                            .AnyAsync(f => f.LoanBookId == loanBook.Id && f.FineType == FineType.LateReturn);

                        if (!fineExists)
                        {
                            var lateFine = new Fine
                            {
                                LoanBookId = loanBook.Id,
                                TotalAmount = (decimal)MinutesLate * libraryPolicySetting.FinePerDay,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "System",
                                FineType = FineType.LateReturn
                            };

                            await unitOfWork.Fines.AddAsync(lateFine);
                        }

                        //  BackgroundJob.Enqueue(() => CalculateFineForReturnedBook(loanBook.Id));

                        loanBook.Status = LoanStatus.PendingFinePayment;
                        message = $"The book was returned {Math.Ceiling(MinutesLate)} minutes late. Lost book fine was cancelled, and late return fine applied.";
                    }
                    else
                    {
                        loanBook.Status = LoanStatus.Closed;
                        message = "The book was returned on time. Lost book fine has been cancelled. No late fine is due.";
                    }

                    await unitOfWork.SaveAsync();

                    await emailService.SendEmailAsync(
                        currentUserService.Email!,
                        "Book Return Update",
                        $"Hello {currentUserService.UserName},\n\n{message}\n\nBook: '{bookCopy.Book.Title}'\nLibrary System");

                    return Result<int>.Success(loanBook.Id);
                }
            }

            var bookId =await  unitOfWork.BookCopies.Query()
                .Where(c => c.Id == request.BookCopyId)
                .Select(c => c.BookId)
                .FirstOrDefaultAsync(cancellationToken);
                

           
            loanBook.ReturnDate = DateTime.Now;
            bookCopy.Status= BookCopyStatus.Available;
            await mediator.Send(new PromoteReservationCommand
            {
                BookId = bookId
            });
            loanBook.Status = LoanStatus.Returned;
            var minutesLate = Math.Max(0, (DateTime.Now - loanBook.DueDate).TotalMinutes);

            if (minutesLate > 0)
            {

                bool fineExists = await unitOfWork.Fines.Query()
                    .AnyAsync(f => f.LoanBookId == loanBook.Id && f.FineType == FineType.LateReturn);

                if (!fineExists)
                {
                    var fine = new Fine
                    {
                        LoanBookId = loanBook.Id,
                        TotalAmount = (decimal)minutesLate * libraryPolicySetting.FinePerDay,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "System",
                        FineType = FineType.LateReturn,
                        DueDate=DateTime.Now.AddMinutes(options1.PaymentGracePeriodDays)
                    };

                    await unitOfWork.Fines.AddAsync(fine);
                }

                loanBook.Status = LoanStatus.PendingFinePayment;
                //var fineDueDate = DateTime.Now.AddDays(options1.PaymentGracePeriodDays);
                var fineDueDate= DateTime.Now.AddMinutes(options1.PaymentGracePeriodDays);
                message = $"The book was returned {Math.Ceiling(minutesLate)} minutes late. A fine has been applied if not already. " +
           $"Please pay the fine by {fineDueDate:MMMM dd, yyyy HH:mm}.";
            }
            else
            {
                loanBook.Status = LoanStatus.Returned;
                message = "The book was returned on time. No fine is due.";
            }

            await unitOfWork.SaveAsync();

            await emailService.SendEmailAsync(
                currentUserService.Email!,
                "Book Return Status",
                $"Hello {currentUserService.UserName},\n\n{message}\n\nLoaned book: '{bookCopy.Book.Title}'\nLibrary System"
            );

            return Result<int>.Success(loanBook.Id);
        }




    }
}
