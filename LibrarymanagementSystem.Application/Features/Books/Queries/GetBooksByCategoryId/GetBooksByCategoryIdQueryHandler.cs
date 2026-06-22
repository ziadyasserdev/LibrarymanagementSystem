using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByCategoryId
{
    public class GetBooksByCategoryIdQueryHandler : IRequestHandler<GetBooksByCategoryIdQuery, Result<List<BookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBooksByCategoryIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<BookDto>>> Handle(GetBooksByCategoryIdQuery request, CancellationToken cancellationToken)
        {

            var category = await unitOfWork.Categories.Query()
                .FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId && !x.IsDeleted);
            if (category == null)

                return Result<List<BookDto>>.Failure(ResultStatus.NotFound, "Category not found");

            var booksDto = await unitOfWork.Books.Query()
      .Where(x => x.CategoryId == request.CategoryId)
      .Select(x => new BookDto
      {
          Id = x.Id,
          Title = x.Title,
          Description = x.Description,
          ISBN = x.ISBN,
          PublishedYear = x.PublishedYear,
          NumberOfPages = x.NumberOfPages,
          Language = x.Language,
          Publisher = x.Publisher.Name,
          Edition = x.Edition,
        
          Price = x.Price,
          
          AuthorName = x.Author.Name,
          CategoryName = x.Category.Name,
          BookFileUrl = x.BookFileUrl,
          
      })
      .ToListAsync();

            if (!booksDto.Any())
                return Result<List<BookDto>>
                    .Failure(ResultStatus.NotFound, "No books found for this category");

            return Result<List<BookDto>>.Success(booksDto);
        }
    }
}
