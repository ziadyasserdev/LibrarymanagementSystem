using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Dtos
{
    public class DamageBookDto
    {
        public int Id { get; set; }
        public int BookCopyId { get; set; }
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public string? UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime? DamageDate { get; set; }
        public string? Notes { get; set; }
    }
}
