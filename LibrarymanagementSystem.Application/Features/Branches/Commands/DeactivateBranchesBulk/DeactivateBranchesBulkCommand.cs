using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.DeactivateBranchesBulk
{
    public class DeactivateBranchesBulkCommand:IRequest<Result<string>>
    {
        public List<int> BranchesId{ get; set; }
        public DeactivateBranchesBulkCommand(List<int> BranchesId)
        {
            this.BranchesId=BranchesId;
        }
    }
}
