using LibrarymanagementSystem.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class FinePayment
    {
        public int Id { get; set; }

        public int FineId { get; set; }
        public Fine Fine { get; set; } = null!;

        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Completed;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentMethod PaymentMethod { get; set; }
    = PaymentMethod.Manual;



        public string? Notes { get; set; }
    }

}
