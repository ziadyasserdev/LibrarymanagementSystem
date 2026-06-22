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

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetAllCopies
{
    public class GetAllCopiesQueryHandler : IRequestHandler<GetAllCopiesQuery, Result<List<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllCopiesQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<object>>> Handle(GetAllCopiesQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.BookCopies.Query()
                .Include(x => x.Book)
                .Include(x => x.Location)
                .ThenInclude(x => x.Branch)
                .AsNoTracking();
            if(currentUserService.IsInRole("Admin"))
            {
                var bookCopies = await query.Select(x => new BookCopyAdminDto
                {
                    Id=x.Id,
                    BookId=x.BookId,
                    Barcode=x.Barcode,
                    BookTitle=x.Book.Title,
                    Status=x.Status.ToString(),
                   
                     CreatedAt=x.CreatedAt,
                     UpdatedAt=x.UpdatedAt,
                     Branch=new BranchAdminDto
                     {
                         Id=x.Location.Branch.Id,
                         Name=x.Location.Branch.Name,
                         Address=x.Location.Branch.Address,
                         IsActive=x.Location.Branch.IsActive,
                         IsDeleted=x.Location.Branch.IsDeleted
                     },
                    IsDeleted=x.IsDeleted,
                    IsDamaged=x.IsDamaged,
                    IsLost=x.IsLost,
                    Location=new LocationAdminDto
                    {
                        Id=x.Location.Id,
                        Floor=x.Location.Floor,
                        Section=x.Location.Section,
                        Shelf=x.Location.Shelf,
                        Capacity=x.Location.Capacity,    
                        IsActive=x.Location.IsActive,
                        IsDeleted=x.Location.IsDeleted
                    }
                }).ToListAsync();

                return Result<List<object>>.Success(bookCopies.Cast<object>().ToList());
            }
            else
            {
                var bookCopies = await query.Select(x => new UserBookCopyDto
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
                return Result<List<object>>.Success(bookCopies.Cast<object>().ToList());
            }
        }
    }
}
