using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.UpdatePublisher
{
    public class UpdatePublisherCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        // Extra Info
        public string? Description { get; set; }
    }
}
