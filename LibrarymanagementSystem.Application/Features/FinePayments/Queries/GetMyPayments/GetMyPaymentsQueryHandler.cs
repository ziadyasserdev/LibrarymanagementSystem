using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetMyPayments
{
    public class GetMyPaymentsQueryHandler : IRequestHandler<GetMyPaymentsQuery, Result<List<UserPaymentHistoryDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyPaymentsQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<UserPaymentHistoryDto>>> Handle(GetMyPaymentsQuery request, CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<UserPaymentHistoryDto>>.Failure(ResultStatus.Unauthorized, "User is not authenticated.");
            var userId=currentUserService.UserId;
            var payments = await unitOfWork.FinePayments.Query()
            .Where(p => p.Fine.LoanBook.UserId == userId)
            .OrderByDescending(p => p.PaymentDate) 
            .Select(p => new UserPaymentHistoryDto
            {
                PaymentId = p.Id,
                FineId = p.FineId,
                FineTitle = p.Fine.FineType.ToString(),
                Amount = p.Amount,
                PaymentDate = p.PaymentDate
            })
            .ToListAsync(cancellationToken);

            return Result<List<UserPaymentHistoryDto>>.Success(payments);
        }
    }
}
