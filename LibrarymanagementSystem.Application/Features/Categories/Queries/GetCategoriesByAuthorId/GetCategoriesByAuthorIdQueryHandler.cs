using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesByAuthorId
{
    public class GetCategoriesByAuthorIdQueryHandler : IRequestHandler<GetCategoriesByAuthorIdQuery, Result<List<CategoryDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCategoriesByAuthorIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesByAuthorIdQuery request, CancellationToken cancellationToken)
        {
            var author = await unitOfWork.Authors.GetByIdAsync(request.AuthorId);
            if (author == null)
                return Result<List<CategoryDto>>.Failure(ResultStatus.NotFound, "Author not found");
            var categories = await unitOfWork.Books.Query()
                .Where(x => x.AuthorId == request.AuthorId)
                .Select(c => c.Category)
                .Distinct()
                .Select(c => new CategoryDto
                {
                    CategoryId=c.CategoryId,
                    Name=c.Name,
                    BookCount=c.Books.Count,
                    Description=c.Description
                }).ToListAsync();
            if(!categories.Any())
                return Result<List<CategoryDto>>.Failure(ResultStatus.NotFound, "Categories not found for this author");
            return Result<List<CategoryDto>>.Success(categories);

        }
    }
}
