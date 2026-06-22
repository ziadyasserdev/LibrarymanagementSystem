using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Locations.Commands.MakeLocationActive;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllActiveLocations
{
    public class GetAllActiveLocationsQuery:IRequest<Result<List<LocationStatusDto>>>
    {
    }
}
