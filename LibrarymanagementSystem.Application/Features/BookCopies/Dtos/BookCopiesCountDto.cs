using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Dtos
{
    public class BookCopiesCountDto
    {
        public int Total { get; set; }
        public int Available { get; set; }
        public int Loaned { get; set; }
        public int Reserved { get; set; }
        public int Lost { get; set; }
        public int Damaged { get; set; }
        public int Deleted { get; set; }
    }
}
