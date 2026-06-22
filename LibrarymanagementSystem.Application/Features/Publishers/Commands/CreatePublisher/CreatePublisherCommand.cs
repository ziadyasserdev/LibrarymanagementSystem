using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.CreatePublisher
{
    public class CreatePublisherCommand:IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }
        public string? Description { get; set; }

    }
}
