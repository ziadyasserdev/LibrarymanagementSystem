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

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetRevenuePayments
{
    public class GetRevenuePaymentsQueryHandler : IRequestHandler<GetRevenuePaymentsQuery, Result<RevenueResponseDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetRevenuePaymentsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<RevenueResponseDto>> Handle(GetRevenuePaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await unitOfWork.FinePayments.Query()
    .Where(x => x.Status != PaymentStatus.Completed
        && x.PaymentDate >= request.From
        && x.PaymentDate <= request.To)
    .Select(x => x.Amount)
    .ToListAsync(cancellationToken);

            var count = payments.Count;
            var total = payments.Sum();
            var average = count == 0 ? 0 : payments.Average();

            var revenuePayments = new RevenueResponseDto
            {
                Month = request.From.Month,
                Year = request.From.Year,
                NumberOfPayments = count,
                TotalRevenue = total,
                AveragePayment = average
            };
            return Result<RevenueResponseDto>.Success(revenuePayments);
        }
    }
}
