using LibrarymanagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Dtos
{
    public class LocationDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
        public int Capacity { get; set; }
        public BranchDto Branch { get; set; } = null!;

        public List<BookInLocationDto> Books { get; set; }
            = new();
    }

    public class BookInLocationDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        
    }
    public class BranchDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }

}
