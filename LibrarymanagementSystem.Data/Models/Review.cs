using LibrarymanagementSystem.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<ReviewReport> Reports { get; set; } = new List<ReviewReport>();
    }
}
