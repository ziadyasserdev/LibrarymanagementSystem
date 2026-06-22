using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.RestorePublisher
{
    public class RestorePublisherCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }

    }
}
