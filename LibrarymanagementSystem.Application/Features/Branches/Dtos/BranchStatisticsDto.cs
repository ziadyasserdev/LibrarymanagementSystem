using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Dtos
{
    public class BranchStatisticsDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }

        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int BorrowedBooks { get; set; }

        public decimal TotalFines { get; set; }

        public string? MostBorrowedBookTitle { get; set; }
        public int MostBorrowedBookCount { get; set; }
    }
}
