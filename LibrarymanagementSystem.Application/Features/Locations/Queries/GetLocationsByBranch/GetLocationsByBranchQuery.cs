using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationsByBranch
{
    public class GetLocationsByBranchQuery:IRequest<Result<List<LocationListDto>>>
    {
        public int BranchId { get; set; }
        public GetLocationsByBranchQuery(int id)
        {
            BranchId=id;
        }
    }
}
