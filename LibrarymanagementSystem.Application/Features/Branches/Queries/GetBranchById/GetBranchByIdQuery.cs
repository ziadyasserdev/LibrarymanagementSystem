using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQuery:IRequest<Result<AdminBranchDto>>
    {
        public int  Id { get; set; }
        public GetBranchByIdQuery(int id)
        {
            Id = id;
        }
    }
}
