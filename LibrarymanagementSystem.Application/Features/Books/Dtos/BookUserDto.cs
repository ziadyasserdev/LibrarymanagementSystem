using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Dtos
{
    public class BookUserDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int PublishedYear { get; set; }
        public decimal Price { get; set; }

        public string AuthorName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string PublisherName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
