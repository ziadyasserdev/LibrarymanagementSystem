using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Dtos
{
    public class FinePaymentsStatisticsDto
    {
        public int TotalFines { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalPending { get; set; }
        public int TotalPartiallyPaid { get; set; }
        public int TotalPaid { get; set; }
        public Dictionary<string, int> FinesByType { get; set; } = new();
        public List<TopUserDto> TopUsers { get; set; } = new(); 
    }

    public class TopUserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalPaid { get; set; }
    }
}
