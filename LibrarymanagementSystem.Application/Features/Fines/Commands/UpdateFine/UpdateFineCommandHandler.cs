using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Commands.UpdateFine
{
    public class UpdateFineCommandHandler : IRequestHandler<UpdateFineCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;

        public UpdateFineCommandHandler(IUnitOfWork unitOfWork,IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
        }
        public async Task<Result<int>> Handle(UpdateFineCommand request, CancellationToken cancellationToken)
        {
            var fine = await unitOfWork.Fines.GetByIdAsync(request.Id);
            if (fine == null)
                return Result<int>.Failure(ResultStatus.NotFound, $"Fine with id {request.Id} not found");

            var loanBook = await unitOfWork.LoanBooks.Query()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == fine.LoanBookId);

            if (loanBook == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Loanbook not found");


            if (request.TotalAmount < fine.PaidAmount)
                return Result<int>.Failure(ResultStatus.Failure,
                    "Total amount cannot be less than the already paid amount.");

          
            fine.TotalAmount = request.TotalAmount;
            fine.Notes = request.Notes;

            var remainingAmount = fine.TotalAmount - fine.PaidAmount;

            if (remainingAmount == 0)
            {
                loanBook.Status = LoanStatus.Closed;

                await emailService.SendEmailAsync(
                    loanBook.User.Email!,
                    "Fine Fully Paid",
                    "Your fine has been fully paid. Thank you."
                );
            }
            else
            {
                loanBook.Status = LoanStatus.PendingFinePayment;

                await emailService.SendEmailAsync(
                    loanBook.User.Email!,
                    "Outstanding Fine Payment",
                    $"You still have {remainingAmount} remaining to pay."
                );
            }

            await unitOfWork.SaveAsync();
            return Result<int>.Success(fine.Id);
        }
    }
}
