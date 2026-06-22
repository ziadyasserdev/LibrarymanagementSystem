using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.SearchBranches
{
    public class SearchBranchesQueryHandler : IRequestHandler<SearchBranchesQuery, Result<PaginatedResult<BranchSearchDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public SearchBranchesQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<PaginatedResult<BranchSearchDto>>> Handle(SearchBranchesQuery request, CancellationToken cancellationToken)
        {
           
            var query =unitOfWork.Branches.Query()
                 .AsNoTracking();
            if(currentUserService.IsInRole("Admin") && request.IsActive.HasValue)
                query=query.Where(x=>x.IsActive== request.IsActive.Value);
            else if (!currentUserService.IsInRole("Admin"))
                query = query.Where(x => x.IsActive && !x.IsDeleted);
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
             
            query = query.Where(c =>
     EF.Functions.Like(c.Name, $"%{request.SearchTerm}%") ||
     EF.Functions.Like(c.Address, $"%{request.SearchTerm}%") ||
     EF.Functions.Like(c.City, $"%{request.SearchTerm}%"));

            if (!string.IsNullOrWhiteSpace(request.City))
                query = query.Where(c => EF.Functions.Like(c.City, $"%{request.City}%"));
            var totalCount = await query.CountAsync(cancellationToken);
            var items=await query.OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BranchSearchDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    City = x.City,
                    IsActive = x.IsActive,
                   Phone = x.PhoneNumber,
                })
                .ToListAsync(cancellationToken);
            return Result<PaginatedResult<BranchSearchDto>>.Success(new PaginatedResult<BranchSearchDto>(items, request.PageNumber, request.PageSize, totalCount));

        }
    }
}
