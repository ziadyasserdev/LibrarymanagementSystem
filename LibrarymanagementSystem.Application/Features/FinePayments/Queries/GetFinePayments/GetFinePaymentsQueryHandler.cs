using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePayments
{
    public class GetFinePaymentsQueryHandler : IRequestHandler<GetFinePaymentsQuery, Result<FinePaymentsResponseDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFinePaymentsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<FinePaymentsResponseDto>> Handle(GetFinePaymentsQuery request, CancellationToken cancellationToken)
        {
            var fine = await unitOfWork.Fines.Query()
      .Where(f => f.Id == request.FineId)
      .Select(f => new FinePaymentsResponseDto
      {
          TotalFineAmount = f.TotalAmount,
          TotalPaid = f.PaidAmount,
          RemainingAmount = f.TotalAmount - f.PaidAmount,
          Payments = f.Payments.Select(p => new FinePaymentHistoryDto
          {
              PaymentId = p.Id,
              Amount = p.Amount,
              PaymentDate = p.PaymentDate
          }).ToList()
      })
      .FirstOrDefaultAsync(cancellationToken);

            if (fine is null)
                return Result<FinePaymentsResponseDto>
                    .Failure(ResultStatus.NotFound, "Fine not found");

            return Result<FinePaymentsResponseDto>.Success(fine);
        }
    }
}
