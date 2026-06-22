using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Dtos
{
    public class LocationCapacityDto
    {
        public int LocationId { get; set; }

        public int TotalCapacity { get; set; }

        public int UsedCapacity { get; set; }

        public int AvailableCapacity { get; set; }
    }
}
