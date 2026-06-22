using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesWithPagination
{
    public class GetBookCopiesWithPaginationQueryHandler
      : IRequestHandler<GetBookCopiesWithPaginationQuery, Result<PaginatedResult<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetBookCopiesWithPaginationQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<object>>> Handle(
            GetBookCopiesWithPaginationQuery request,
            CancellationToken cancellationToken)
        {
            var query = unitOfWork.BookCopies.Query();

          
            var totalCount = currentUserService.IsInRole("Admin")
                ? await query.CountAsync(cancellationToken)
                : await query.Where(x => !x.IsDeleted).CountAsync(cancellationToken);

            if (totalCount == 0)
                return Result<PaginatedResult<object>>.Failure(ResultStatus.NotFound, "BookCopies not found");

            if (currentUserService.IsInRole("Admin"))
            {
                var bookCopies = await query
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
                        IsDeleted = x.IsDeleted,
                        IsDamaged = x.IsDamaged,
                        IsLost = x.IsLost,
                        Branch = new BranchAdminDto
                        {
                            Id = x.Location.Branch.Id,
                            Name = x.Location.Branch.Name,
                            Address = x.Location.Branch.Address,
                            IsActive = x.Location.Branch.IsActive,
                            IsDeleted = x.Location.Branch.IsDeleted
                        },
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
                    }).ToListAsync(cancellationToken);

                return Result<PaginatedResult<object>>.Success(
                    new PaginatedResult<object>(bookCopies, request.PageNumber, request.PageSize, totalCount));
            }
            else
            {
                var bookCopies = await query
                    .Where(x => !x.IsDeleted)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new UserBookCopyDto
                    {
                        Id = x.Id,
                        Barcode = x.Barcode,
                        BookTitle = x.Book.Title,
                        Status = x.Status.ToString(),
                        CreatedAt = x.CreatedAt,
                        IsLost = x.IsLost,
                        Branch = new BranchDto
                        {
                            Id = x.Location.Branch.Id,
                            Name = x.Location.Branch.Name,
                            Address = x.Location.Branch.Address,
                        },
                        Location = new LocationDto
                        {
                            Id = x.Location.Id,
                            Floor = x.Location.Floor,
                            Section = x.Location.Section,
                            Shelf = x.Location.Shelf,
                        }
                    }).ToListAsync(cancellationToken);

                return Result<PaginatedResult<object>>.Success(
                    new PaginatedResult<object>(bookCopies, request.PageNumber, request.PageSize, totalCount));
            }
        }
    }
    
}
