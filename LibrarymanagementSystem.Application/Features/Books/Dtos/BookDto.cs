using LibrarymanagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public int PublishedYear { get; set; }
        public int NumberOfPages { get; set; }
        public string Language { get; set; }
        public string Publisher { get; set; }
        public string Edition { get; set; }
       
     
        public decimal Price { get; set; }
    
        public string? BookFileUrl { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
    }
}
