using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentSummary
{
    public class GetFinePaymentSummaryQueryHandler : IRequestHandler<GetFinePaymentSummaryQuery, Result<FinePaymentSummaryDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFinePaymentSummaryQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<FinePaymentSummaryDto>> Handle(GetFinePaymentSummaryQuery request, CancellationToken cancellationToken)
        {
            var fine = await unitOfWork.Fines.Query()
      .Where(x => x.Id == request.FineId)
      .Select(x => new FinePaymentSummaryDto
      {
          TotalFineAmount = x.TotalAmount,
          TotalPaid = x.PaidAmount,
          RemainingAmount = Math.Max(x.TotalAmount - x.PaidAmount, 0),
          Status = x.PaidAmount == 0
                      ? PaymentProgress.Pending
                      : x.PaidAmount >= x.TotalAmount
                          ? PaymentProgress.Paid
                          : PaymentProgress.PartiallyPaid
      })
      .FirstOrDefaultAsync(cancellationToken);

            if (fine == null)
                return Result<FinePaymentSummaryDto>.Failure(ResultStatus.NotFound, "Fine not found");

            return Result<FinePaymentSummaryDto>.Success(fine);
        }
    }
}
