using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByCategoryName
{
    public class GetBooksByCategoryNameQueryHandler : IRequestHandler<GetBooksByCategoryNameQuery, Result<List<BookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBooksByCategoryNameQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<BookDto>>> Handle(GetBooksByCategoryNameQuery request, CancellationToken cancellationToken)
        {
            var categoryId = await unitOfWork.Categories.Query()
                .Where(x => x.Name.ToLower() == request.CategoryName.ToLower() && !x.IsDeleted)
                .Select(x => x.CategoryId).FirstOrDefaultAsync();
            if (categoryId == 0)
                return Result<List<BookDto>>.Failure(ResultStatus.NotFound, "Category not found.");
            var books = await unitOfWork.Books.Query()
                .Where(x => x.CategoryId == categoryId && !x.IsDeleted)
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

            if (!books.Any())
                return Result<List<BookDto>>.Failure(ResultStatus.NotFound, "No books found for the specified author.");
         
            return Result<List<BookDto>>.Success(books);

        }
    }
}
