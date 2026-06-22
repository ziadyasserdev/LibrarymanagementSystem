using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.CreateBranch
{
    
    public class CreateBranchCommand:IRequest<Result<CreateBranchDto>>
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public string Address { get; set; }
        public string City { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
