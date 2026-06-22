using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Shelf { get; set; }
        public string Floor { get; set; }
        public string Section { get; set; }
        public int Capacity { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        public ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();
    }

}
