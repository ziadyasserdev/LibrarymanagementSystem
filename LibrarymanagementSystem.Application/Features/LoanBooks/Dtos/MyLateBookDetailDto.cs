using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Dtos
{
    public class MyLateBookDetailDto
    {
       
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
      public int BookCopyId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
       
    }
}
