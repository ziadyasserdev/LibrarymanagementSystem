using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Dtos
{
    public class CanDeleteLocationDto
    {
        public int LocationId { get; set; }

        public bool CanDelete { get; set; }

        public string? Reason { get; set; }
    }
}
