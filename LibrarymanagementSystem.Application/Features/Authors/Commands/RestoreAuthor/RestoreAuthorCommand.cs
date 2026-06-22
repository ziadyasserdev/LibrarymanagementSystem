using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.RestoreAuthor
{
    public class RestoreAuthorCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public RestoreAuthorCommand(int id)
        {
            Id=id;
        }
    }
}
