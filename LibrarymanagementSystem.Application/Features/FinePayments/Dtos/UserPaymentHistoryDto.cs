using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Dtos
{

    public class UserPaymentHistoryDto
    {
        public int PaymentId { get; set; }
        public int FineId { get; set; }
        public string FineTitle { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }

}

