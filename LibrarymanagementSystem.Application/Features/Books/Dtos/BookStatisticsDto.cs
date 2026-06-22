using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Dtos
{
    public class BookStatisticsDto
    {
       
        public int TotalBooks { get; set; }
        public int ActiveBooks { get; set; }
        public int InactiveBooks { get; set; }
        public int DeletedBooks { get; set; }

       
        public List<AggregationDto> BooksPerCategory { get; set; } = new();

    
        public List<AggregationDto> BooksPerAuthor { get; set; } = new();

       
        public List<AggregationDto> BooksPerPublisher { get; set; } = new();

       
        public List<AggregationDto> Top5MostExpensiveBooks { get; set; } = new();
        public List<AggregationDto> Top5BooksWithMostPages { get; set; } = new();
        public double AverageBookPrice { get; set; }
        public double AverageNumberOfPages { get; set; }
        public double AverageRating { get; set; } 
    }

  
    public class AggregationDto
    {
        public string Name { get; set; } = "";
        public int Count { get; set; }
        public decimal? AveragePrice { get; set; }
        public double? AveragePages { get; set; }
        public double? AverageRating { get; set; }
    }
}
