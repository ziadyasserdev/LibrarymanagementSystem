using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Commands.ConfirmFinePayment
{
    public class ConfirmFinePaymentCommandHandler : IRequestHandler<ConfirmFinePaymentCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public ConfirmFinePaymentCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(ConfirmFinePaymentCommand request, CancellationToken cancellationToken)
        {
           var finePayment=await unitOfWork.FinePayments.Query()
                .FirstOrDefaultAsync(x => x.Id == request.FinePaymentId);
            if (finePayment is null)
                return Result<int>.Failure(ResultStatus.NotFound, "FinePayment not found");
            if (finePayment.Status != PaymentStatus.Pending)
                return Result<int>.Failure(ResultStatus.Conflict, "Payment is not in a pending state.");
            switch (request.Success)
            {
                case true:
                    finePayment.Status = PaymentStatus.Completed;
                    break;
                    case false:
                    finePayment.Status= PaymentStatus.Failed;
                    break;
            }

            await unitOfWork.SaveAsync();
            return Result<int>.Success(finePayment.Id);
        }
    }
}
