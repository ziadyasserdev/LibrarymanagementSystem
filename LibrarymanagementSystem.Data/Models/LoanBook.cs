using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class LoanBook
    {

        public int Id { get; set; }

        public DateTime LoanDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public LoanStatus Status { get; set; } = LoanStatus.Active;

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

       
        public int BookCopyId { get; set; }
        public BookCopy BookCopy { get; set; } = null!;

       
        public Fine? Fine { get; set; }



    }

}
