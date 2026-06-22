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

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByPublisherId
{
    public class GetBooksByPublisherIdQueryHandler : IRequestHandler<GetBooksByPublisherIdQuery, Result<List<BookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBooksByPublisherIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<BookDto>>> Handle(GetBooksByPublisherIdQuery request, CancellationToken cancellationToken)
        {
            var publisher = await unitOfWork.Publishers.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
            if (publisher == null)
                return Result<List<BookDto>>.Failure(ResultStatus.NotFound, "Publisher not found or deleted");
          
            var booksDto = await unitOfWork.Books.Query()
    .Where(c => c.PublisherId == request.Id)
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
    .AsNoTracking()
    .ToListAsync(cancellationToken);

            return Result<List<BookDto>>.Success(booksDto);

        }
    }
}
