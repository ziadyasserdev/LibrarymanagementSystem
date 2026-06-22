using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Dtos
{
    public class FineDto
    {
        public int Id { get; set; }

        public int LoanBookId { get; set; }

        public string BookTitle { get; set; }=string.Empty;
        public string UserId {  get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; } = 0;

        public FineType FineType { get; set; } 

        public LoanStatus LoanStatus { get; set; } 

        public DateTime CreatedAt { get; set; } 

        public decimal RemainingAmount => TotalAmount - PaidAmount; 
    }
}
