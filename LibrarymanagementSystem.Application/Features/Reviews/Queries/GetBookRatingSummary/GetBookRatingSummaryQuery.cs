using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetBookRatingSummary
{
    public class GetBookRatingSummaryQuery:IRequest<Result<BookRatingSummaryDto>>
    {
        public int Id { get; set; }
        public GetBookRatingSummaryQuery(int id)
        {
           Id = id;
        }
    }
}
