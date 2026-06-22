using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Dtos
{
    public class BookCopyAdminDto
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = null!;
        public string Status { get; set; } = null!;
        public bool IsLost { get; set; }
        public bool IsDamaged { get; set; }
        public bool IsDeleted { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = null!;
        public LocationAdminDto Location { get; set; } = null!;
        public BranchAdminDto Branch { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class LocationAdminDto : LocationDto
    {
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BranchAdminDto : BranchDto
    {
        public string Address { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
