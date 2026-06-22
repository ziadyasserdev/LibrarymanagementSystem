using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Dtos
{
    public class BranchCountDto
    {
        public int TotalBranches { get; set; }

        public int ActiveBranches { get; set; }

        public int InactiveBranches { get; set; }

       
    }
}
