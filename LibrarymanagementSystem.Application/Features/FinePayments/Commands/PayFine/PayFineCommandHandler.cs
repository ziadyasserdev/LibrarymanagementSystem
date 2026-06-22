using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Commands.PayFine
{
    public class PayFineCommandHandler : IRequestHandler<PayFineCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IEmailService emailService;

        public PayFineCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.emailService = emailService;
        }
        public async Task<Result<string>> Handle(PayFineCommand request, CancellationToken cancellationToken)
        {


            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User is not authenticated.");


            var fine = await unitOfWork.Fines.Query()
                .Include(f => f.LoanBook)
                .ThenInclude(x => x.BookCopy)
                    .ThenInclude(x => x.Book)
                .Include(f => f.LoanBook.User)
                .FirstOrDefaultAsync(f => f.Id == request.FineId, cancellationToken);

            if (fine == null)
                return Result<string>.Failure(ResultStatus.NotFound, "This fine was not found.");


            if (fine.LoanBook.UserId != currentUserService.UserId)
                return Result<string>.Failure(ResultStatus.Unauthorized, "This fine does not belong to the current user.");


            if (fine.PaidAmount >= fine.TotalAmount)
                return Result<string>.Failure(ResultStatus.Conflict, "This fine has already been fully paid.");

            decimal remainingAmount = fine.TotalAmount - fine.PaidAmount;
            decimal actualPaidAmount;

            if (request.PayStatus == PayStatus.PayPartial)
            {
                if (request.Amount >= remainingAmount)
                    return Result<string>.Failure(ResultStatus.Conflict,
                        "You are trying to pay the full fine using partial payment. Use PayFull instead.");

                actualPaidAmount = Math.Min(request.Amount, remainingAmount);
            }
            else
            {
                if (request.Amount < remainingAmount)
                    return Result<string>.Failure(
                        ResultStatus.Conflict,
                        $"Partial payment is not allowed. Remaining balance is {remainingAmount:C}. Please pay the full outstanding amount."
                    );


                actualPaidAmount = remainingAmount;
            }


            fine.PaidAmount += actualPaidAmount;
            remainingAmount = fine.TotalAmount - fine.PaidAmount;


            //fine.LoanBook.Status = remainingAmount == 0 ? LoanStatus.Closed : LoanStatus.PendingFinePayment;
            if (fine.LoanBook.Status == LoanStatus.Lost)
            {

                fine.LoanBook.Status = LoanStatus.Lost;
            }
            else
            {

                fine.LoanBook.Status = remainingAmount == 0 ? LoanStatus.Closed : LoanStatus.PendingFinePayment;
            }


            var finePayment = new FinePayment
            {
                FineId = fine.Id,
                Amount = actualPaidAmount,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = request.PaymentMethod,

            };

            switch (request.PaymentMethod)
            {
                case PaymentMethod.Manual:
                    finePayment.Status = PaymentStatus.Completed;
                    break;
                case PaymentMethod.Card:
                    finePayment.Status = PaymentStatus.Pending;
                    break;
                case PaymentMethod.Online:
                    finePayment.Status = PaymentStatus.Pending;
                    break;
                case PaymentMethod.Cash:
                    finePayment.Status = PaymentStatus.Completed;
                    break;
            }


            await unitOfWork.FinePayments.AddAsync(finePayment);
            await unitOfWork.SaveAsync();


            if (!string.IsNullOrWhiteSpace(fine.LoanBook.User?.Email))
            {
                string emailBody = $"Hello {fine.LoanBook.User.UserName},<br/><br/>" +
                                   $"You have paid {actualPaidAmount:C} towards your fine.<br/>" +
                                   $"Remaining balance: {remainingAmount:C}.<br/>" +
                                   $"Loaned book: '{fine.LoanBook.BookCopy.Book.Title}'<br/>" +
                                   $"Thank you for your payment.<br/><br/>Library System";

                await emailService.SendEmailAsync(fine.LoanBook.User.Email, "Library Fine Payment Confirmation", emailBody);
            }

            return Result<string>.Success("Payment successful. Check your email for details.");
        }
    }
}


//    var userId = currentUserService.UserId;
//    if (!currentUserService.IsAuthenticated)
//        return Result<string>.Failure(ResultStatus.Unauthorized, "User is not authenticated.");

//    if (!await unitOfWork.LoanBooks.HasFines(userId!))
//        return Result<string>.Failure(ResultStatus.Conflict, "The user has no fines.");


//    var fine = await unitOfWork.Fines.GetByIdAsync(request.FineId);
//    if (fine == null)
//        return Result<string>.Failure(ResultStatus.NotFound, "This fine not found");

//    var loanBook = await unitOfWork.LoanBooks.Query()
//        .Include(c => c.Book)
//        .FirstOrDefaultAsync(v => v.Id == fine.LoanBookId);
//    if(loanBook == null)
//        return Result<string>.Failure(ResultStatus.NotFound, "This loanbook not found");
//    if (loanBook.Status == LoanStatus.Closed)
//        return Result<string>.Failure(ResultStatus.Conflict, "This fine is already paid");


//    decimal remainingAmount = fine.TotalAmount - fine.PaidAmount;
//    decimal actualPaidAmount;

//    if (request.PayStatus == PayStatus.PayPartial)
//    {
//        if (request.Amount >= remainingAmount)
//            return Result<string>.Failure(ResultStatus.Conflict, "You are trying to pay full fine using partial payment. Use PayFull instead.");

//        actualPaidAmount = Math.Min(request.Amount, remainingAmount);
//    }
//    else
//    {
//        if (request.Amount < remainingAmount)
//            return Result<string>.Failure(ResultStatus.Conflict, $"Total fine amount remaining is {remainingAmount:C}, you need to pay full amount");

//        actualPaidAmount = remainingAmount;
//    }



//    fine.PaidAmount += actualPaidAmount;
//    remainingAmount = fine.TotalAmount - fine.PaidAmount;
//    loanBook.Status = remainingAmount == 0 ? LoanStatus.Closed : LoanStatus.PendingFinePayment;


//    var finePayment = new FinePayment
//    {
//        FineId = fine.Id,
//        Amount = actualPaidAmount,
//        PaymentDate = DateTime.UtcNow,
//        PaymentMethod = request.PaymentMethod
//    };
//    await unitOfWork.FinePayments.AddAsync(finePayment);


//    await emailService.SendEmailAsync(
//        currentUserService.Email!,
//        "Library Fine Payment Status",
//        $"Hello {currentUserService.UserName},\n\n" +
//        $"You paid {actualPaidAmount:C}. Remaining fine: {remainingAmount:C}.\n" +
//        $"Loaned book: '{loanBook.Book.Title}'\n" +
//        $"Thank you for your payment.\n\n" +
//        $"Library System");


//    await unitOfWork.SaveAsync();

//    return Result<string>.Success("Payment successful. Check your email for details.");
//}