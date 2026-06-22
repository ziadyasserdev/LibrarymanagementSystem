using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFinesByDate
{
    public class GetFinesByDateQuery : IRequest<Result<PaginatedResult<FineByDateDto>>>
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetFinesByDateQuery(DateTime from, DateTime to, int pageNumber, int pageSize)
        {
            From = from;
            To = to;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
