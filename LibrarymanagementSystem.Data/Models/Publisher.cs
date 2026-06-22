using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class Publisher
    {
        public int Id { get;  set; }

        public string Name { get;  set; }
        public string Email { get;  set; }
        public string Phone { get;  set; }
        public string Website { get;  set; }


        // Address
        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        // Extra Info
        public string? Description { get; set; }

        public string? LogoUrl { get; set; }


        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public string? DeletedBy { get; set; }





        public bool IsActive { get;  set; } = true;
        public bool IsDeleted { get;  set; } = false;

      

        public virtual ICollection<Book> Books { get;  set; } = new List<Book>();
    }
}
