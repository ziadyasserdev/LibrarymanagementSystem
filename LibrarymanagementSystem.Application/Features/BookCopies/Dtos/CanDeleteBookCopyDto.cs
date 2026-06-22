using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Dtos
{
    public class CanDeleteBookCopyDto
    {
        public int Id { get; set; }
        public bool CanDelete { get; set; }
        public string? Reason { get; set; }
    }
}
