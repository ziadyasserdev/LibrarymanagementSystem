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

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesByAuthorName
{
    public class GetCategoriesByAuthorNameQueryHandler : IRequestHandler<GetCategoriesByAuthorNameQuery, Result<List<CategoryDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCategoriesByAuthorNameQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesByAuthorNameQuery request, CancellationToken cancellationToken)
        {
            var author = await unitOfWork.Authors.Query()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == request.AuthorName.ToLower());
            if (author == null)
                return Result<List<CategoryDto>>.Failure(ResultStatus.NotFound, "Author not found");
            var categories = await unitOfWork.Books.Query()
                .Where(x => x.Author.Name == request.AuthorName)
                .Select(x => x.Category)
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
