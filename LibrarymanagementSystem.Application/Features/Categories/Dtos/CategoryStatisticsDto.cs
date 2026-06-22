using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Dtos
{
    public class CategoryStatisticsDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public int TotalBooks { get; set; }
        public int ActiveBooks { get; set; }
        public int DeletedBooks { get; set; }

     
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int BorrowedCopies { get; set; }
        public int LostCopies { get; set; }

      
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }

     
        public int DistinctAuthors { get; set; }

        public decimal? AveragePrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

     
        public int TotalBorrowedTimes { get; set; }

      
        public Dictionary<string, int>? BooksByLanguage { get; set; }
        public Dictionary<int, int>? BooksByYear { get; set; }

     
        public double? AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<TopBookDto> TopBooks { get; set; } = new();
    }

    public class TopBookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int BorrowCount { get; set; }
    }
}