using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Dtos
{
    public class RevenueResponseDto
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public decimal TotalRevenue { get; set; }

        public int NumberOfPayments { get; set; }

        public decimal AveragePayment { get; set; }
    }
}
