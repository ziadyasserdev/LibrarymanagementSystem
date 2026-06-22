using LibrarymanagementSystem.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Dtos
{
    public class MyFineDto
    {


        public string BookTitle { get; set; } = string.Empty;

        public FineType FineType { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal RemainingAmount => TotalAmount - PaidAmount;

        public bool IsPaid => PaidAmount >= TotalAmount;

        public DateTime CreatedAt { get; set; }

        public DateTime? LoanDate
        {
            get; set;
        }
    }
}
