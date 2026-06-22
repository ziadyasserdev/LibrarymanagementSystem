using LibrarymanagementSystem.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Dtos
{
    public class FinePaymentSummaryDto
    {
        public decimal TotalFineAmount { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal RemainingAmount { get; set; }

        public PaymentProgress Status { get; set; }
    }
}
