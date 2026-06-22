using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class Branch
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }

        public string Address { get; set; }
        public string City { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}
