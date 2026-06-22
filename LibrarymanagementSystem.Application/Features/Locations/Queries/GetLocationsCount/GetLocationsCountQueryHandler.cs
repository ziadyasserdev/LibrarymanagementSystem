using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationsCount
{
    public class GetLocationsCountQueryHandler : IRequestHandler<GetLocationsCountQuery, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLocationsCountQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(GetLocationsCountQuery request, CancellationToken cancellationToken)
        {
           var locations=await unitOfWork.Locations.Query()
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancellationToken);
            var count = locations.Count;
            return Result<string>.Success(count.ToString());    
        }
    }
}
