using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesByUserId
{
    public class GetAllFinesByUserIdQuery:IRequest<Result<List<FineDto>>>
    {
        public string UserId { get; set; }
        public GetAllFinesByUserIdQuery(string id)
        {
            this.UserId = id;
        }
    }
}
