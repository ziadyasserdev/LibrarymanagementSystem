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

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetAllBooks
{
    public class GetAllBookQueryHandler : IRequestHandler<GetAllBookQuery, Result<List<BookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllBookQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<BookDto>>> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            var books=await unitOfWork.Books.Query()
                .Include(x=>x.Author)
                .Include(x=>x.Category)
               
                .Include(v=>v.Publisher)
               .Where(x => !x.IsDeleted && x.IsActive)
                .ToListAsync();
            if (books==null||!books.Any())
                return Result<List<BookDto>>.Failure(ResultStatus.NotFound, "No books found");
            var booksDto = books.Select(x => new BookDto
            {
                Id=x.Id,
                Title=x.Title,
                Description=x.Description,
                ISBN=x.ISBN,
                PublishedYear=x.PublishedYear,
                NumberOfPages=x.NumberOfPages,
                Language=x.Language,
                Publisher=x.Publisher.Name,
                Edition=x.Edition,
              
                Price=x.Price,
              
                
                AuthorName=x.Author.Name,
                CategoryName=x.Category.Name,
                BookFileUrl=x.BookFileUrl,
               


            }).ToList();
             return Result<List<BookDto>>.Success(booksDto);
        }
    }
}
