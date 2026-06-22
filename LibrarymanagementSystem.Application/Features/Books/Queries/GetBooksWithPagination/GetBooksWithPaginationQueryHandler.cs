using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksWithPagination
{
    public class GetBooksWithPaginationQueryHandler : IRequestHandler<GetBooksWithPaginationQuery, Result<PaginatedResult<BookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBooksWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
     
        public async Task<Result<PaginatedResult<BookDto>>> Handle(GetBooksWithPaginationQuery request, CancellationToken cancellationToken)
        {

            var query=await unitOfWork.Books.Query()
                .AsNoTracking()
                .OrderBy(b => b.Id)
                 .Skip((request.PageNumber - 1) * request.PageSize)
           .Take(request.PageSize)
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
               CategoryName = b.Category.Name,


           })
           .ToListAsync();


       var totalCount = await unitOfWork.Books.Query().CountAsync();
            var paginatedResult = new PaginatedResult<BookDto>(
     query,
     request.PageNumber,
     request.PageSize,
     totalCount
 );

            return Result<PaginatedResult<BookDto>>.Success(paginatedResult);
        }
    }
}
