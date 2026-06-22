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

namespace LibrarymanagementSystem.Application.Features.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Result<BookDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateBookCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
   
        public async Task<Result<BookDto>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var category=await unitOfWork.Categories.Query()
                .FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId && !x.IsDeleted);
            if (category == null)
                return Result<BookDto>.Failure(ResultStatus.NotFound, "Category not found or deleted");
            var author = await unitOfWork.Authors.Query()
                .FirstOrDefaultAsync(x => x.AuthorId == request.AuthorId && !x.IsDeleted && x.IsActive);
            if (author == null)
                return Result<BookDto>.Failure(ResultStatus.NotFound, "Author not found or deleted or inactive now");

            var publisher = await unitOfWork.Publishers.Query()
              .FirstOrDefaultAsync(x => x.Id == request.PublisherId
              && !x.IsDeleted && x.IsActive);
            if (publisher == null)
                return Result<BookDto>.Failure(ResultStatus.NotFound, "Publisher not found");





            var book =await unitOfWork.Books.Query()
              
                .FirstOrDefaultAsync(i=>i.Id == request.Id && !i.IsDeleted);
            if (book == null)
            {
                return Result<BookDto>.Failure(ResultStatus.NotFound, "Book not found");
            }
          





            book.Title=request.Title;
            book.Description=request.Description;
            book.ISBN=request.ISBN;
            book.PublishedYear=request.PublishedYear;
            book.NumberOfPages=request.NumberOfPages;
            book.Language=request.Language;
            book.PublisherId=request.PublisherId;
            book.Edition=request.Edition;
          
            book.Price=request.Price;
           
            book.AuthorId=request.AuthorId;
            book.CategoryId=request.CategoryId;
            await unitOfWork.SaveAsync();


            var bookDto = await unitOfWork.Books.Query()
              .Where(b => b.Id == book.Id)
              .Select(b => new BookDto
              {
                  Id = b.Id,
                  Title = b.Title,
                  Description = b.Description,
                  ISBN = b.ISBN,
                  PublishedYear = b.PublishedYear,
                  NumberOfPages = b.NumberOfPages,
                  Language = b.Language,
                  Publisher = b.Publisher.Name,
                  Edition = b.Edition,
                  Price = b.Price,
                  BookFileUrl = b.BookFileUrl,
                  AuthorName = b.Author.Name,
                  CategoryName = b.Category.Name
              })
              .FirstOrDefaultAsync(cancellationToken);

            return Result<BookDto>.Success(bookDto!);
        }
    }
}
