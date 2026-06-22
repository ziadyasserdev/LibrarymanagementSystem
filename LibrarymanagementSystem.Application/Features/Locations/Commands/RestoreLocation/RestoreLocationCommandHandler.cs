using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.RestoreLocation
{
    public class RestoreLocationCommandHandler : IRequestHandler<RestoreLocationCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public RestoreLocationCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(RestoreLocationCommand request, CancellationToken cancellationToken)
        {
            var location=await unitOfWork.Locations.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (location == null) 
                return Result<int>.Failure(ResultStatus.NotFound, "Location not found");
            if(!location.IsDeleted)
                return Result<int>.Failure(ResultStatus.Failure, "Cannot restore a location that is not deleted");
            location.IsDeleted= false;
            location.DeletedAt = null;
            location.UpdatedAt=DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(location.Id);    
        }
    }
}
