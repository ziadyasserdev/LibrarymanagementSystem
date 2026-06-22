using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.DeleteLocation
{
    public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteLocationCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await unitOfWork.Locations.Query()
                .Include(x => x.BookCopies)
                .FirstOrDefaultAsync(x => x.Id == request.LocationId && !x.IsDeleted);
            if (location == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Location not found");
            if (location.BookCopies.Any())
                return Result<int>.Failure(ResultStatus.Failure, "Cannot delete location with associated books");
            location.IsDeleted = true;
            location.IsActive = false;
            location.DeletedAt = DateTime.Now;
            location.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(location.Id);
        }
    }
}
