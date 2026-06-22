using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchesStatistics
{
    public class GetBranchesStatisticsQueryHandler : IRequestHandler<GetBranchesStatisticsQuery, Result<BranchCountDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBranchesStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<BranchCountDto>> Handle(GetBranchesStatisticsQuery request, CancellationToken cancellationToken)
        {
          
            var result = await unitOfWork.Branches.Query()
     .Where(x => !x.IsDeleted)
     .GroupBy(x => 1)
     .Select(g => new BranchCountDto
     {
         TotalBranches = g.Count(),
         ActiveBranches = g.Count(x => x.IsActive),
         InactiveBranches = g.Count(x => !x.IsActive)
     })
     .FirstOrDefaultAsync(cancellationToken);

            return Result<BranchCountDto>.Success(result ?? new BranchCountDto());
        }
    }
}
