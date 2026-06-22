using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Result<BookDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBookByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<BookDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
                .Include(c => c.Author)
                .Include(c => c.Category)
                
           
                .Include(b=>b.Publisher)
                  .Where(x => !x.IsDeleted && x.IsActive)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (book == null)
                return Result<BookDto>.Failure(ResultStatus.NotFound, "Book not found");
            var bookDto = new BookDto
            {
                Id=request.Id,
                Title=book.Title,
                Description=book.Description,
                ISBN=book.ISBN,
                PublishedYear=book.PublishedYear,
                NumberOfPages=book.NumberOfPages,
                Language=book.Language,
                Publisher=book.Publisher.Name,
                Edition=book.Edition,
               
                Price=book.Price,
              
                BookFileUrl =book.BookFileUrl,
                AuthorName=book.Author.Name,
                CategoryName=book.Category.Name,
              
            };
             return Result<BookDto>.Success(bookDto);
        }
    }
}
