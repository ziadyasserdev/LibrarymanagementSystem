using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.RestoreBranch
{
    public class RestoreBranchCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
}
