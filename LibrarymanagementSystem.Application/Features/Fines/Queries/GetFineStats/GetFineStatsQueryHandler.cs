using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineStats
{
    public class GetFineStatsQueryHandler : IRequestHandler<GetFineStatsQuery, Result<FineStatsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFineStatsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<FineStatsDto>> Handle(GetFineStatsQuery request, CancellationToken cancellationToken)
        {
            var stats = await unitOfWork.Fines
        .Query()
        .GroupBy(x => 1)
        .Select(x => new FineStatsDto
        {
            TotalFines = x.Count(),
            TotalPaidFines = x.Count(f => f.PaidAmount >= f.TotalAmount),
            TotalUnpaidFines = x.Count(f => f.PaidAmount < f.TotalAmount),
            TotalRevenue = x.Sum(f => f.PaidAmount)
        })
        .FirstOrDefaultAsync(cancellationToken) ?? new FineStatsDto();

            return Result<FineStatsDto>.Success(stats);
        }
    }
}
