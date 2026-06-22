using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Dtos
{
    public class UserBookCopyDto
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = null!;
        public string Status { get; set; } = null!;
        public bool IsLost { get; set; }
        public string BookTitle { get; set; } = null!;
        public LocationDto Location { get; set; } = null!;
        public BranchDto Branch { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class LocationDto
    {
        public int Id { get; set; }
        public string Shelf { get; set; } = null!;
        public string Floor { get; set; } = null!;
        public string Section { get; set; } = null!;
    }

    public class BranchDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
      public  string Address { get; set; } = null!;
    }
}
