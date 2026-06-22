using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime? UpdatedAt { get;  set; }
        public DateTime? DeletedAt { get;  set; }
        public bool IsDeleted { get;  set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
