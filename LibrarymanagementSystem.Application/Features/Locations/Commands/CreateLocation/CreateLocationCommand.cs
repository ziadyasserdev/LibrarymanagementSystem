using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.CreateLocation
{
    public class CreateLocationCommand:IRequest<Result<CreateLocationDto>>
    {
        public string Shelf { get; set; }
        public string Floor { get; set; }
        public string Section { get; set; }
        public int Capacity { get; set; }
        public int BranchId { get; set; }
    }
}
