using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetAllPublishers
{
    public class GetAllPublishersQuery:IRequest<Result<List<PublisherReadDto>>>
    {
    }
}
