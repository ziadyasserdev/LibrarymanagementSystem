using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Dtos
{
    public class BranchDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int BooksCount { get; set; }
     
        public int LoansCount { get; set; }

        public bool IsActive { get; set; }
    }
}
