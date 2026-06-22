using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.BulkRestoreLocations
{
    public class BulkRestoreLocationsCommand:IRequest<Result<string>>
    {
        public List<int> LocationIds { get; set; }
        public BulkRestoreLocationsCommand(List<int> LocationIds)
        {
            this.LocationIds = LocationIds;
        }
    }
}
