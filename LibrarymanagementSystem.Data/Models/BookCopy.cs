using LibrarymanagementSystem.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class BookCopy
    {
        public int Id { get; set; }

      
        public string Barcode { get; set; } = null!;

       
        public BookCopyStatus Status { get; set; } = BookCopyStatus.Available;

     
        public bool IsLost { get; set; } = false;
        public bool IsDamaged { get; set; } = false;

     
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

      
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

      
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;

      
        public ICollection<LoanBook> LoanBooks { get; set; }
            = new List<LoanBook>();
    }
}
