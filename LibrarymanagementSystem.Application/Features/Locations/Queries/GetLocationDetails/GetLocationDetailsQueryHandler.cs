using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationDetails
{
    public class GetLocationDetailsQueryHandler : IRequestHandler<GetLocationDetailsQuery, Result<LocationDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLocationDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<LocationDetailsDto>> Handle(GetLocationDetailsQuery request, CancellationToken cancellationToken)
        {
            var locationDetails = await unitOfWork.Locations.Query()
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .Select(x => new LocationDetailsDto
                {
                    Id = x.Id,
                    Name = x.Section,
                    IsActive = x.IsActive,
                    Capacity = x.Capacity,
                    Branch = new BranchDto
                    {
                        Id = x.BranchId,
                        Name = x.Branch.Name,
                    },

                    Books = x.BookCopies.Select(x => new BookInLocationDto
                    {
                        Id = x.BookId,
                        Title = x.Book.Title,
                       
                    }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
            if (locationDetails == null)
                return Result<LocationDetailsDto>.Failure(ResultStatus.NotFound, "Location Details not found");
            return Result<LocationDetailsDto>.Success(locationDetails);

        }
    }
}
