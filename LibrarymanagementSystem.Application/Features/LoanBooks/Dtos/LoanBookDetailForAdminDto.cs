using LibrarymanagementSystem.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Dtos
{
    public class LoanBookDetailForAdminDto
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime DueDate { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public LoanStatus LoanStatus {  get; set; }
        public bool HasFines { get; set; }
        public bool IsReturned { get; set; }
    }
}
