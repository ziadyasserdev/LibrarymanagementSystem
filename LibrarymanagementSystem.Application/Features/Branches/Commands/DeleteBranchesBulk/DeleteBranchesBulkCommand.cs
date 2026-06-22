using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.DeleteBranchesBulk
{
    public class DeleteBranchesBulkCommand:IRequest<Result<string>>
    {
        public List<int> BranchIds { get; set; }
        public DeleteBranchesBulkCommand(List<int> BranchIds)
        {
            this.BranchIds = BranchIds;
        }
    }
}
