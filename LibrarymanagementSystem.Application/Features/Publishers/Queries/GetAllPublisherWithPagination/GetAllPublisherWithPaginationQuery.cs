using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetAllPublisherWithPagination
{
    public class GetAllPublisherWithPaginationQuery:IRequest<Result<PaginatedResult<PublisherReadDto>>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public PublisherStatusFilter Status { get; set; } = PublisherStatusFilter.All;

        public GetAllPublisherWithPaginationQuery(int pageNum,int pageSiz, PublisherStatusFilter Status)
        {
            this.PageNumber = pageNum;
            this.PageSize = pageSiz;
            this.Status = Status;
        }
    }
}
