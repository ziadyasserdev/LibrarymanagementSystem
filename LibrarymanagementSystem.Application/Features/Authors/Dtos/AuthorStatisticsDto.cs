using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Dtos
{

    public class AuthorStatisticsDto
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }

        // ======== Books Info ========
        public int TotalBooks { get; set; }
        public int ActiveBooks { get; set; }
        public int DeletedBooks { get; set; }

        // ======== Copies Info ========
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int BorrowedCopies { get; set; }
        public int ReservedCopies { get; set; }
        public int LostCopies { get; set; }

        // ======== Ratings & Reviews ========
        public double? AverageRating { get; set; }
        public int TotalReviews { get; set; }

        // ======== Breakdown Info ========
        public Dictionary<int, int>? BooksByCategory { get; set; }
        public Dictionary<int, int>? BooksByYear { get; set; }
        public Dictionary<string, int>? BooksByLanguage { get; set; }

        // ======== Author Status ========
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // ======== Top Books ========
        public List<TopBookDto> TopBooks { get; set; } = new();

        // ======== Loan History ========
        public List<LoanHistoryDto> LoanHistory { get; set; } = new();
    }

    public class TopBookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int BorrowCount { get; set; }
        public double? AverageRating { get; set; }
    }

    public class LoanHistoryDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int LoanedCount { get; set; }
        public int ReturnedCount { get; set; }
        public int OverdueCount { get; set; }
    }

}