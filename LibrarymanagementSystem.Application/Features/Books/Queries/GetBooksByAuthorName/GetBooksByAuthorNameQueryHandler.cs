using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByAuthorName
{
    public class GetBooksByAuthorNameQueryHandler : IRequestHandler<GetBooksByAuthorNameQuery, Result<List<BookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBooksByAuthorNameQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<BookDto>>> Handle(GetBooksByAuthorNameQuery request, CancellationToken cancellationToken)
        {
            var authorId = await unitOfWork.Authors.Query()
                .Where(x => x.Name == request.AuthorName && !x.IsDeleted && x.IsActive)
                .Select(x => x.AuthorId)
                .FirstOrDefaultAsync();
            var books=await unitOfWork.Books.Query()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Publisher)
                .Where(x => x.AuthorId == authorId && !x.IsDeleted)
                .ToListAsync();
            if (!books.Any())
                return Result<List<BookDto>>.Failure(ResultStatus.NotFound, "No books found for the specified author.");
            var booksDto = books.Select(x => new BookDto
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
               


            }).ToList();
            return Result<List<BookDto>>.Success(booksDto);
        }
    }
}
