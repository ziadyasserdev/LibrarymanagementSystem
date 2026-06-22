using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.RestoreBranchesBulk
{
    public class RestoreBranchesBulkCommand:IRequest<Result<string>>
    {
        public List<int> BranchIds { get; set; }
        public RestoreBranchesBulkCommand(List<int> BranchIds)
        {
            this.BranchIds = BranchIds;
        }
    }
}
