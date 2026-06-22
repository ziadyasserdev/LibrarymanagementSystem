using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.SearchLocations
{
    public class SearchLocationsQuery:IRequest<Result<List<LocationSearchDto>>>
    {
        public string? Shelf { get; set; }

        public string? Floor { get; set; }

        public string? Section { get; set; }

        public int? BranchId { get; set; }
    }
}
