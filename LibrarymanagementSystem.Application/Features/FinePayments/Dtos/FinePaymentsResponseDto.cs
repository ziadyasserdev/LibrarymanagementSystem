using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Dtos
{
    public class FinePaymentsResponseDto
    {
        public decimal TotalFineAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingAmount { get; set; }

        public List<FinePaymentHistoryDto> Payments { get; set; }= new List<FinePaymentHistoryDto>();
    }
    public class FinePaymentHistoryDto
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
