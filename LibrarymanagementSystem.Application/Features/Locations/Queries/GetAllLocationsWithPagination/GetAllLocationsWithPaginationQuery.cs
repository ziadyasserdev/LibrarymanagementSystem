using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllLocationsWithPagination
{
    public class GetAllLocationsWithPaginationQuery:IRequest<Result<PaginatedResult<LocationDto>>>
    {
        public int  PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
