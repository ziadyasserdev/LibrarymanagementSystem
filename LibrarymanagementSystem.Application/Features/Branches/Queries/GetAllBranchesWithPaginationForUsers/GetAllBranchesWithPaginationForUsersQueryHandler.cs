using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesWithPaginationForUsers
{
    public class GetAllBranchesWithPaginationForUsersQueryHandler : IRequestHandler<GetAllBranchesWithPaginationForUsersQuery, Result<PaginatedResult<BranchUserDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        
        public GetAllBranchesWithPaginationForUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<BranchUserDto>>> Handle(GetAllBranchesWithPaginationForUsersQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Branches.Query()
                .Where(x => !x.IsDeleted && x.IsActive)
                .AsNoTracking();
            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{request.Search}%") 
                || EF.Functions.Like(x.City, $"%{request.Search}%"));
            if(!string.IsNullOrEmpty(request.City))
                query = query.Where(x => EF.Functions.Like(x.City, $"%{request.City}%"));


            query = (request.BranchFilter, request.BranchSort) switch
            {
                (BranchFilter.ByName, BranchSort.Descending)
                    => query.OrderByDescending(x => x.Name),

                (BranchFilter.ByCity, BranchSort.Ascending)
                    => query.OrderBy(x => x.City),

                (BranchFilter.ByCity, BranchSort.Descending)
                    => query.OrderByDescending(x => x.City),

                _ => query.OrderBy(x => x.Name) 
            };

            var totalCount = await query.CountAsync();
                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new BranchUserDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Address = x.Address,
                        City = x.City,
                    }).ToListAsync();

            return Result<PaginatedResult<BranchUserDto>>.Success(new PaginatedResult<BranchUserDto>(items, request.PageNumber, request.PageSize, totalCount));


        }
    }
}
