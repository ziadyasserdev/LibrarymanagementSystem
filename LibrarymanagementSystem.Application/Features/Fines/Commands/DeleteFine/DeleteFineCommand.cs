using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Commands.DeleteFine
{
    public class DeleteFineCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DeleteFineCommand(int id)
        {
            this.Id = id;
        }

    }
}
