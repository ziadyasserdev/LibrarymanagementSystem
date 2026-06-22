using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.UpdateLocation
{
    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateLocationCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await unitOfWork.Locations.GetByIdAsync(request.Id);
            if (location == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Location not found");
            if (await unitOfWork.Locations.IsDuplicateLocationExclusive(request.Id,request.Shelf, request.Floor, request.Section))
                return Result<int>.Failure(ResultStatus.Failure, "This location already exists.");

            var branchExist = await unitOfWork.Branches.Query()
              .FirstOrDefaultAsync(c => c.Id == request.BranchId && !c.IsDeleted && c.IsActive);
            if (branchExist == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Branch not found.");





            location.Floor=request.Floor;
            location.Section=request.Section;
            location.Shelf = request.Shelf;
            location.BranchId = request.BranchId;
            location.UpdatedAt = DateTime.Now;
            location.Capacity = request.Capacity;
            await unitOfWork.SaveAsync();
          
            return Result<int>.Success(location.Id);
        }
    }
}
