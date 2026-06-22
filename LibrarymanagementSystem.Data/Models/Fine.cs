using LibrarymanagementSystem.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class Fine
    {
        public int Id { get; set; }

        public int LoanBookId { get; set; }
        public LoanBook LoanBook { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; } = 0;

        public FineType FineType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = "System";

        public string? Notes { get; set; }

        public ICollection<FinePayment> Payments { get; set; }
            = new List<FinePayment>();
        public DateTime DueDate { get; set; }
        public bool IsPaid => PaidAmount >= TotalAmount;
    }

}
