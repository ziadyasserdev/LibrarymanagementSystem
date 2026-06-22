using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.BulkActivateLocations
{
    public class BulkActivateLocationsCommand:IRequest<Result<string>>
    {
        public List<int> LocationIds { get; set; }
        public BulkActivateLocationsCommand(List<int> LocationIds)
        {
            this.LocationIds = LocationIds;
        }
    }
}
