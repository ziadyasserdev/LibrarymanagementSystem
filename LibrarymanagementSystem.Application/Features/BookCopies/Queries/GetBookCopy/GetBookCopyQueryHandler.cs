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

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopy
{
    public class GetBookCopyQueryHandler : IRequestHandler<GetBookCopyQuery, Result<object>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetBookCopyQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<object>> Handle(GetBookCopyQuery request, CancellationToken cancellationToken)
        {
            
            var query = unitOfWork.BookCopies.Query();
                
            if(currentUserService.IsInRole("Admin"))
            {
                var bookCopy = await query.Where(x => x.Id == request.Id)
                    .Select(x => new BookCopyAdminDto
                    {
                        Id=x.Id,
                        Barcode=x.Barcode,
                        Status=x.Status.ToString(),
                        IsLost=x.IsLost,
                        IsDamaged=x.IsDamaged,
                        IsDeleted=x.IsDeleted,
                        BookId=x.BookId,
                        BookTitle=x.Book.Title,
                        Location=new LocationAdminDto
                        {
                            Id=x.LocationId,
                            Floor=x.Location.Floor,
                            Section=x.Location.Section,
                            Shelf=x.Location.Shelf, 
                            Capacity=x.Location.Capacity,
                            IsActive=x.Location.IsActive,
                            IsDeleted=x.Location.IsDeleted
                           
                        },
                        Branch=new BranchAdminDto
                        {
                            Id=x.Location.BranchId,
                            Name=x.Location.Branch.Name,
                            Address=x.Location.Branch.Address,
                            IsActive=x.Location.Branch.IsActive,
                            IsDeleted= x.Location.Branch.IsDeleted
                        },
                        CreatedAt=x.CreatedAt,
                        UpdatedAt=x.UpdatedAt
                      
                    }).FirstOrDefaultAsync(cancellationToken);

                if (bookCopy == null)
                    return Result<object>.Failure(ResultStatus.NotFound, "BookCopy not found");
                return Result<object>.Success(bookCopy);
            }
            else
            {
                var bookCopy = await query.Where(x => x.Id == request.Id 
                && !x.IsDeleted)
                   .Select(x => new UserBookCopyDto
                   {
                       Id = x.Id,
                       Barcode = x.Barcode,
                       Status = x.Status.ToString(),
                       IsLost = x.IsLost,
                       BookTitle = x.Book.Title,
                       Location = new LocationDto
                       {
                           Id = x.LocationId,
                           Floor = x.Location.Floor,
                           Section = x.Location.Section,
                           Shelf = x.Location.Shelf,
                         

                       },
                       Branch = new BranchDto
                       {
                           Id = x.Location.BranchId,
                           Name = x.Location.Branch.Name,
                           Address = x.Location.Branch.Address,
                        
                       },
                       CreatedAt = x.CreatedAt,
                      

                   }).FirstOrDefaultAsync(cancellationToken);

                if (bookCopy == null)
                    return Result<object>.Failure(ResultStatus.NotFound, "BookCopy not found");
                return Result<object>.Success(bookCopy);
            }
                
        }
    }
}
