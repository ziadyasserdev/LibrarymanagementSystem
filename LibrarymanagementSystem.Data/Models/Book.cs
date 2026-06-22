using LibrarymanagementSystem.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public int PublishedYear { get; set; }
        public int NumberOfPages { get; set; }
        public string Language { get; set; }
        public string Edition { get; set; }
        public decimal Price { get; set; }

        public string? BookFileUrl { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<BookCopy> Copies { get; set; }
            = new List<BookCopy>();

        public ICollection<Review> Reviews { get; set; }
            = new List<Review>();
        public ICollection<Reservation> Reservations { get; set; }
    = new List<Reservation>();

    }
}
