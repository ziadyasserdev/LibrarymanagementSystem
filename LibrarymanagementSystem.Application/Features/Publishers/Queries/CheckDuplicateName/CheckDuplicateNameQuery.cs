using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.CheckDuplicateName
{
    public class CheckDuplicateNameQuery:IRequest<Result<bool>>
    {
        public string Name{ get; set; }
        public CheckDuplicateNameQuery(string n)
        {
            Name= n;
        }
    }
}
