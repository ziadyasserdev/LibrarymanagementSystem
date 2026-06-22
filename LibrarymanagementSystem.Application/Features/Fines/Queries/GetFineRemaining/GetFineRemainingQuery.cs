using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineRemaining
{
    public class GetFineRemainingQuery : IRequest<Result<FineRemainingDto>>
    {
        public int FineId { get; set; }

        public GetFineRemainingQuery(int fineId)
        {
            FineId = fineId;
        }
    }
}
