using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.MakeLocationInActive
{
    public class MakeLocationInActiveCommand:IRequest<Result<int>>
    {
        public int LocationId { get; set; }
    }
}
