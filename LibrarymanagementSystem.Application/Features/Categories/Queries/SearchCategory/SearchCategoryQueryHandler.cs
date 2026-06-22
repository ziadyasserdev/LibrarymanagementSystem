using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.SearchCategory
{
    public class SearchCategoryQueryHandler : IRequestHandler<SearchCategoryQuery, Result<PaginatedResult<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public SearchCategoryQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<PaginatedResult<object>>> Handle(SearchCategoryQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Categories.Query();
            if(currentUserService.IsInRole("Admin"))
            {
                if(request.IncludeDeleted.HasValue) 
                    query = query.Where(x => x.IsDeleted == request.IncludeDeleted.Value);
            }
            else
            {
                query = query.Where(x => !x.IsDeleted); 
            }
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(term) || x.Description.ToLower().Contains(term));
            }
            query = request.categorySort switch
            {
                CategorySort.ByCreatedAt => request.Descending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                _ => request.Descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
            };

            var totalCount = await query.CountAsync(cancellationToken);
            var pagedQuery = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);


            if (currentUserService.IsInRole("Admin"))
            {
                var data = await pagedQuery.Select(x => new CategorySearchAdminDto
                {
                   CategoryId=x.CategoryId,
                   Name=x.Name,
                    Description =x.Description,
                     IsDeleted = x.IsDeleted,
                     CreatedAt=x.CreatedAt,
                }).ToListAsync(cancellationToken);

                return Result<PaginatedResult<object>>.Success(new PaginatedResult<object>(data, request.PageNumber, request.PageSize, totalCount));
            }
            else
            {
                var data = await pagedQuery.Select(x => new CategorySearchDto
                {
                  CategoryId=x.CategoryId,
                   Name=x.Name,
                    Description =x.Description,
                    CreatedAt=x.CreatedAt,
                }).ToListAsync(cancellationToken);

                return Result<PaginatedResult<object>>.Success(new PaginatedResult<object>(data, request.PageNumber, request.PageSize, totalCount));
            }
        }
    }
}
