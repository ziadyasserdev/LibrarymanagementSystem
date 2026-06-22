using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesWithPagination
{
    public class GetCategoriesWithPaginationQueryHandler : IRequestHandler<GetCategoriesWithPaginationQuery, Result<PaginatedResult<CategoryDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCategoriesWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<CategoryDto>>> Handle(GetCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Categories.Query()
                .Include(c=>c.Books)
                .AsNoTracking();
            var categoriesCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.CategoryId)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CategoryDto
                {
                    CategoryId=x.CategoryId,
                    Name=x.Name,
                    Description=x.Description,
                    BookCount=x.Books.Count
                }).ToListAsync();
            var paginatedResult=new PaginatedResult<CategoryDto>(items,  request.PageNumber, request.PageSize, categoriesCount);
            return Result<PaginatedResult<CategoryDto>>.Success(paginatedResult);
        }
    }
}
