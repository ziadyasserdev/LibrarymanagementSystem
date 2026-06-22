using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Dtos
{
    public class LocationSearchDto
    {
        public int Id { get; set; }

        public string Shelf { get; set; }

        public string Floor { get; set; }

        public string Section { get; set; }

        public string BranchName { get; set; }

        public int BooksCount { get; set; }
    }
}
