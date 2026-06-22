using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesByBook
{
    public class GetBookCopiesByBookQueryHandler : IRequestHandler<GetBookCopiesByBookQuery, Result<PaginatedResult<BookCopyAdminDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBookCopiesByBookQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<BookCopyAdminDto>>> Handle(GetBookCopiesByBookQuery request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
                .FirstOrDefaultAsync(x => x.Id == request.BookId && !x.IsDeleted, cancellationToken);
            if (book == null)
                return Result<PaginatedResult<BookCopyAdminDto>>.Failure(ResultStatus.NotFound, "Book not found");
           
            var query = unitOfWork.BookCopies.Query()
                .Include(c => c.Location)
                .ThenInclude(l => l.Branch)
                .Where(c => c.BookId == request.BookId);
            switch (request.Status)
            {
                case BookCopyStatus.Available:
                    query=query.Where(x => x.Status == BookCopyStatus.Available && !x.IsDeleted);
                    break;
              
                case BookCopyStatus.Loaned:
                    query = query.Where(x => x.Status == BookCopyStatus.Loaned && !x.IsDeleted);
                    break;
                case BookCopyStatus.Damaged:
                    query = query.Where(x => x.Status == BookCopyStatus.Damaged && !x.IsDeleted);
                    break;
                case BookCopyStatus.Reserved:
                    query = query.Where(x => x.Status == BookCopyStatus.Reserved && !x.IsDeleted);
                    break;
                case BookCopyStatus.Lost:
                    query = query.Where(x => x.Status == BookCopyStatus.Lost && !x.IsDeleted);
                    break;
                case BookCopyStatus.Maintenance:
                    query = query.Where(x => x.Status == BookCopyStatus.Maintenance && !x.IsDeleted);
                    break;
            }

        

            if (request.IncludeDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == request.IncludeDeleted.Value);
            if (request.LocationId.HasValue)
                query = query.Where(x => x.LocationId == request.LocationId.Value);

            if (request.BranchId.HasValue)
                query = query.Where(x => x.Location.BranchId == request.BranchId.Value);

            if(request.SortBy != null)
            {
                switch(request.SortBy.ToLower())
                {
                    case "id":
                        query = request.SortDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                        break;
                    case "createdat":
                        query = request.SortDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt);
                        break;
                    case "updatedat":
                        query = request.SortDescending ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt);
                        break;
                    default:
                        query = request.SortDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                        break;
                }
            }


            var bookCopies = await query
                .Where(x => x.BookId == book.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BookCopyAdminDto
                {
                    Id = x.Id,
                    BookId = x.BookId,
                    Barcode = x.Barcode,
                    BookTitle = x.Book.Title,
                    Status = x.Status.ToString(),

                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Branch = new BranchAdminDto
                    {
                        Id = x.Location.Branch.Id,
                        Name = x.Location.Branch.Name,
                        Address = x.Location.Branch.Address,
                        IsActive = x.Location.Branch.IsActive,
                        IsDeleted = x.Location.Branch.IsDeleted
                    },
                    IsDeleted = x.IsDeleted,
                    IsDamaged = x.IsDamaged,
                    IsLost = x.IsLost,
                    Location = new LocationAdminDto
                    {
                        Id = x.Location.Id,
                        Floor = x.Location.Floor,
                        Section = x.Location.Section,
                        Shelf = x.Location.Shelf,
                        Capacity = x.Location.Capacity,
                        IsActive = x.Location.IsActive,
                        IsDeleted = x.Location.IsDeleted

                    }
                }).ToListAsync();
            var totalCount = await query.CountAsync(cancellationToken);
            return Result<PaginatedResult<BookCopyAdminDto>>.Success(new PaginatedResult<BookCopyAdminDto>(bookCopies, request.PageNumber,request.PageSize,totalCount));
            

        }
    }
}
