using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorStatistics
{
    public class GetAuthorStatisticsQuery : IRequest<Result<AuthorStatisticsDto>>
    {
        public int AuthorId { get; set; }
        public GetAuthorStatisticsQuery(int authorId)
        {
            AuthorId = authorId;
        }
    }
}
