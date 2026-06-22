using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesOfCurrentUser
{
    public class GetAllFinesOfCurrentUserQuery:IRequest<Result<List<MyFineDto>>>
    {
        public FineStatus FineStatus { get; set; }
        public GetAllFinesOfCurrentUserQuery(FineStatus fineStatus)
        {
            FineStatus = fineStatus;
        }
    }
}
