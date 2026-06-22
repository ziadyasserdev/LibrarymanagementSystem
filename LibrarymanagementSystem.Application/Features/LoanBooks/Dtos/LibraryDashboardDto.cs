using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Dtos
{
    public class LibraryDashboardDto
    {
        public int ActiveLoans { get; set; }
        public int LateLoans { get; set; }
        public int LostBooks { get; set; }
        public decimal TotalFinesCollected { get; set; }
        public string MostBorrowedBook { get; set; } = string.Empty;
    }
}
