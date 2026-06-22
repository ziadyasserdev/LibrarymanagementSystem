using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Dtos
{
    public class FinePaymentDto
    {
        public int Id { get; set; }

        public int FineId { get; set; }
      

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } 
        public PaymentMethod PaymentMethod { get; set; }
    = PaymentMethod.Manual;

       

        public string? Notes { get; set; }
    }
}
