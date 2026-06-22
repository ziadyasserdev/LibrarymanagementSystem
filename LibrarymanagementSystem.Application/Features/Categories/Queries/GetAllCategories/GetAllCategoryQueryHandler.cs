using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, Result<List<CategoryDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllCategoryQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.Categories.Query()
                .Include(c => c.Books)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
                
            if (categories == null || !categories.Any())
                return Result<List<CategoryDto>>.Failure(ResultStatus.NotFound, "No categories found.");
            var categoriesDto = categories.Select(x => new CategoryDto
            {
                CategoryId=x.CategoryId,
                Name=x.Name,
                Description=x.Description,
                BookCount=x.Books.Count,
            }).ToList();

            return Result<List<CategoryDto>>.Success(categoriesDto);
        }
    }
}
