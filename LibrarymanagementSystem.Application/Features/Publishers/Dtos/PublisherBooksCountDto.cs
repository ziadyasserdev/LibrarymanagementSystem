using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Dtos
{
    public class PublisherBooksCountDto
    {
        public int PublisherId { get; set; }

        public string PublisherName { get; set; }

        public int BooksCount { get; set; }
    }
}
