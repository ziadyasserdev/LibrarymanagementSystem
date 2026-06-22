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

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentsStatistics
{
    public class GetFinePaymentsStatisticsQueryHandler : IRequestHandler<GetFinePaymentsStatisticsQuery, Result<FinePaymentsStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFinePaymentsStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<FinePaymentsStatisticsDto>> Handle(GetFinePaymentsStatisticsQuery request, CancellationToken cancellationToken)
        {
            var fines = unitOfWork.Fines.Query()
                .Include(x => x.LoanBook)
                .ThenInclude(x => x.User)
                .AsNoTracking();
            var totalFines = await fines.CountAsync();
            var totalRevenue = await fines.SumAsync(x => x.PaidAmount);
            var totalPending = await fines.Where(x => x.PaidAmount == 0).CountAsync();
            var totalPartiallyPaid = await fines.Where(x => x.PaidAmount > 0 && x.PaidAmount < x.TotalAmount).CountAsync();
            var totalPaid = await fines.Where(x => x.PaidAmount >= x.TotalAmount).CountAsync();
            var finesByType = await fines
      .GroupBy(x => x.FineType)
      .Select(g => new
      {
          FineType = g.Key,
          Count = g.Count()
      })
      .ToDictionaryAsync(x => x.FineType.ToString(), x => x.Count);

            var TopUsers = fines.GroupBy(f => f.LoanBook.User)
                             .Select(g => new TopUserDto
                             {
                                 UserId = g.Key.Id,
                                 UserName = g.Key.FullName,
                                 TotalPaid = g.Sum(f => f.PaidAmount)
                             })
                             .OrderByDescending(u => u.TotalPaid)
                             .Take(5)
                             .ToList();
        


        var statistics = new FinePaymentsStatisticsDto
            {
                TotalFines = totalFines,
                TotalRevenue = totalRevenue,
                TotalPending = totalPending,
                TotalPartiallyPaid = totalPartiallyPaid,
                TotalPaid = totalPaid,
               FinesByType = finesByType,
                TopUsers =TopUsers
            };

            return Result<FinePaymentsStatisticsDto>.Success(statistics);

        }
    }
}
