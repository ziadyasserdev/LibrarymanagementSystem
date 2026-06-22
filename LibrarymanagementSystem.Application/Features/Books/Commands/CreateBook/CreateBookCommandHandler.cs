using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.CreateBook
{
  
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<BookDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateBookCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<BookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.Query()
                .FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId && !x.IsDeleted);
            if (category == null)
                return Result<BookDto>.Failure(ResultStatus.NotFound, "Category not found or deleted");
            var author = await unitOfWork.Authors.Query()
                .FirstOrDefaultAsync(x => x.AuthorId == request.AuthorId && !x.IsDeleted && x.IsActive);
            if (author == null)
                return Result<BookDto>.Failure(ResultStatus.NotFound, "Author not found");

            var publisher = await unitOfWork.Publishers.Query()
                .FirstOrDefaultAsync(x => x.Id == request.PublisherId
                && !x.IsDeleted && x.IsActive);
            if (publisher == null)
                return Result<BookDto>.Failure(ResultStatus.NotFound, "Publisher not found");

          


            var book = new Book
            {
                Title = request.Title,
                Description = request.Description,
                ISBN = request.ISBN,
                PublishedYear = request.PublishedYear,
                NumberOfPages = request.NumberOfPages,
                Language = request.Language,
                PublisherId = request.PublisherId,
                Edition = request.Edition,
               
                Price = request.Price,
              
                CategoryId = request.CategoryId,
                AuthorId = request.AuthorId
            };
            await unitOfWork.Books.AddAsync(book);
            await unitOfWork.SaveAsync();
            var createdBook = await unitOfWork.Books.Query()
    .Include(x => x.Author)
    .Include(x => x.Category)
    .Include(x => x.Publisher)
    
    .FirstAsync(x => x.Id == book.Id);
            var bookDto = new BookDto
            {
                Id = createdBook.Id,
                Title = createdBook.Title,
                Description = createdBook.Description,
                ISBN = createdBook.ISBN,
                PublishedYear = createdBook.PublishedYear,
                NumberOfPages = createdBook.NumberOfPages,
                Language = createdBook.Language,
                Publisher = createdBook.Publisher.Name,
                Edition = createdBook.Edition,
              
                Price = createdBook.Price,

              
                BookFileUrl = createdBook.BookFileUrl,

                AuthorName = createdBook.Author.Name,
                CategoryName = createdBook.Category.Name,

                
            };

            return Result<BookDto>.Success(bookDto);


        }
    }
}
