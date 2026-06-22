using LibrarymanagementSystem.Application.Common.PaginatedResults;
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

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetDeletedBranches
{
    public class GetDeletedBranchesQueryHandler : IRequestHandler<GetDeletedBranchesQuery, Result<PaginatedResult<DeletedBranchDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetDeletedBranchesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

       
        public async Task<Result<PaginatedResult<DeletedBranchDto>>> Handle(GetDeletedBranchesQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Branches.Query()
                .Where(x => x.IsDeleted)
                .AsNoTracking();
            var totalCount=await query.CountAsync();
            var items = await query.OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new DeletedBranchDto
                {
                    Id=x.Id,
                    Name=x.Name,
                    Address=x.Address,
                    City=x.City,
                    DeletedAt=x.DeletedAt,
                }).ToListAsync();

            return Result<PaginatedResult<DeletedBranchDto>>.Success(new PaginatedResult<DeletedBranchDto>(items, request.PageNumber, request.PageSize, totalCount));
        }
    }
}
