using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorByCategoryName
{
    public class GetAuthorByCategoryNameQueryHandler : IRequestHandler<GetAuthorByCategoryNameQuery, Result<List<AuthorDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAuthorByCategoryNameQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<AuthorDto>>> Handle(GetAuthorByCategoryNameQuery request, CancellationToken cancellationToken)
        {
            var categoryId = await unitOfWork.Categories.GetCategoryIdByName(request.CategoryName);
            if (categoryId == 0)
            {
                return Result<List<AuthorDto>>.Failure(ResultStatus.Failure, "Category not found");
            }
            var books = await unitOfWork.Books.Query()
                .Where(b => b.CategoryId == categoryId && !b.IsDeleted)
                .ToListAsync(cancellationToken);

            if ( !books.Any())
            {
                return Result<List<AuthorDto>>.Failure(
                    ResultStatus.Failure,
                    "No books found for this category"
                );
            }

            var authors = books
              .Where(b => b.Author != null)
              .Select(b => b.Author)
              .Distinct()
              .Select(author => new AuthorDto
              {
                  AuthorId=author.AuthorId,
                    Name=author.Name,
                    Biography=author.Biography,
                    DateOfBirth=author.DateOfBirth,
              })
              .ToList();




            return Result<List<AuthorDto>>.Success(authors);
        }
        }
}
