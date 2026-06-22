using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.BulkSoftDeleteLocations
{
    public class BulkSoftDeleteLocationsCommand:IRequest<Result<string>>
    {
        public List<int> LocationIds { get; set; }
        public BulkSoftDeleteLocationsCommand(List<int> LocationIds)
        {
            this.LocationIds = LocationIds;
        }
    }
}
