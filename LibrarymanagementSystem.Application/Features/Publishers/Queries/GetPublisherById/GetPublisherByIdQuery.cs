using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherById
{
    public class GetPublisherByIdQuery:IRequest<Result<PublisherDetailDto>>
    {
        public int Id { get; set; }
        public GetPublisherByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}
