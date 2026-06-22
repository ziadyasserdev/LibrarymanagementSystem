using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Dtos
{
    public class FineByDateDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal RemainingAmount { get; set; }

      

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }

        public int BorrowRecordId { get; set; }
    }
}
