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

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.CreateLocation
{
    public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, Result<CreateLocationDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateLocationCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<CreateLocationDto>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var branchExist = await unitOfWork.Branches.Query()
                .FirstOrDefaultAsync(c => c.Id == request.BranchId && !c.IsDeleted && c.IsActive);
                if (branchExist == null)
                return Result<CreateLocationDto>.Failure(ResultStatus.NotFound, "Branch not found.");
            

            if (await unitOfWork.Locations.IsDuplicateLocation(request.Shelf, request.Floor, request.Section))
                return Result<CreateLocationDto>.Failure(ResultStatus.Failure, "This location already exists.");

            var loction = new Location
            {
                Section = request.Section,
                Shelf= request.Shelf,
                Floor= request.Floor,
                BranchId= request.BranchId,
                Capacity=request.Capacity,
            };
            await unitOfWork.Locations.AddAsync(loction);
            await unitOfWork.SaveAsync();
            var locationDto = new CreateLocationDto
            {
                Id=loction.Id,
                Section=loction.Section,
                Shelf=loction.Shelf,
                Floor=loction.Floor,
                BranchId=loction.BranchId,
                Capacity=loction.Capacity

            };

            return Result<CreateLocationDto>.Success(locationDto);
        }
    }
}
