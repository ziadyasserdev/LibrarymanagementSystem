using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.MakeLocationInActive
{
    public class MakeLocationInActiveCommandHandler : IRequestHandler<MakeLocationInActiveCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public MakeLocationInActiveCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(MakeLocationInActiveCommand request, CancellationToken cancellationToken)
        {
            var location = await unitOfWork.Locations.Query()
                .FirstOrDefaultAsync(x => x.Id == request.LocationId && !x.IsDeleted, cancellationToken);

            if (location == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Location not found");

            if (!location.IsActive)
                return Result<int>.Failure(ResultStatus.Failure, "Location is already inactive");

            location.IsActive = false;
            location.UpdatedAt = DateTime.Now;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(location.Id);
        }
    }
}
