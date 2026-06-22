using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Dtos
{
    public class FineStatsDto
    {
        public int TotalFines { get; set; }

        public decimal TotalRevenue { get; set; }

        public int TotalPaidFines { get; set; }

        public int TotalUnpaidFines { get; set; }
    }
}
