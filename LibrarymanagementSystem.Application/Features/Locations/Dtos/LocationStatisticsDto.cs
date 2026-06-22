using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Dtos
{
    public class LocationStatisticsDto
    {
        public int TotalLocations { get; set; }

        public int ActiveLocations { get; set; }

        public int DeletedLocations { get; set; }

        public int EmptyLocations { get; set; }

        public int TotalBooks { get; set; }
       // public int Capacity { get; set; }
    }
}
