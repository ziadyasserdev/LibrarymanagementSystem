using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
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

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetMyPaymentSummary
{
    public class GetMyPaymentSummaryQueryHandler : IRequestHandler<GetMyPaymentSummaryQuery, Result<List<UserPaymentSummaryDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyPaymentSummaryQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<UserPaymentSummaryDto>>> Handle(GetMyPaymentSummaryQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<UserPaymentSummaryDto>>.Failure(ResultStatus.Unauthorized, "User is not authenticated.");
            var fines=await unitOfWork.Fines.Query()
                .Where(x => x.LoanBook.UserId == currentUserService.UserId)
                .Select(x => new UserPaymentSummaryDto
                {
                    FineId = x.Id,
                    FineTitle=x.FineType.ToString(),
                    TotalFineAmount=x.TotalAmount,
                    TotalPaid=x.PaidAmount,
                    RemainingAmount =Math.Max(x.TotalAmount - x.PaidAmount, 0),
                    Status = x.PaidAmount == 0
                      ? PaymentProgress.Pending
                      : x.PaidAmount >= x.TotalAmount
                          ? PaymentProgress.Paid
                          : PaymentProgress.PartiallyPaid
                }).ToListAsync(cancellationToken);  
            if(fines ==null)
                return Result<List<UserPaymentSummaryDto>>.Failure(ResultStatus.NotFound, "No fines found for the current user.");
            return Result<List<UserPaymentSummaryDto>>.Success(fines);
        }
    }
}
