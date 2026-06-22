using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetAvailableBookCopies
{
    public class GetAvailableBookCopiesQueryHandler : IRequestHandler<GetAvailableBookCopiesQuery, Result<PaginatedResult<UserBookCopyDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAvailableBookCopiesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<UserBookCopyDto>>> Handle(GetAvailableBookCopiesQuery request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
              .FirstOrDefaultAsync(x => x.Id == request.BookId && !x.IsDeleted, cancellationToken);
            if (book == null)
                return Result<PaginatedResult<UserBookCopyDto>>.Failure(ResultStatus.NotFound, "Book not found");

            var query = unitOfWork.BookCopies.Query()
             .Include(c => c.Location)
             .ThenInclude(l => l.Branch)
             .Where(c => c.BookId == request.BookId && c.Status == BookCopyStatus.Available && !c.IsDeleted);

            var bookCopies = await query
           
             .Skip((request.PageNumber - 1) * request.PageSize)
             .Take(request.PageSize)
             .Select(x => new UserBookCopyDto
             {
                 Id = x.Id,
            
                 Barcode = x.Barcode,
                 BookTitle = x.Book.Title,
                 Status = x.Status.ToString(),

                 CreatedAt = x.CreatedAt,
              
                 Branch = new BranchDto
                 {
                     Id = x.Location.Branch.Id,
                     Name = x.Location.Branch.Name,
                     Address = x.Location.Branch.Address,
                 
                 },
               
                 IsLost = x.IsLost,
                 Location = new LocationDto
                 {
                     Id = x.Location.Id,
                     Floor = x.Location.Floor,
                     Section = x.Location.Section,
                     Shelf = x.Location.Shelf,
                   

                 }
             }).ToListAsync();
            var totalCount = await query.CountAsync(cancellationToken);
            return Result<PaginatedResult<UserBookCopyDto>>.Success(new PaginatedResult<UserBookCopyDto>(bookCopies, request.PageNumber, request.PageSize, totalCount));

        }
    }
}
