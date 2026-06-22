using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.CheckAuthorExistsByName
{
    public class CheckAuthorExistsByNameQuery : IRequest<Result<bool>>
    {
        public string Name { get; set; }

        public CheckAuthorExistsByNameQuery(string name)
        {
            Name = name;
        }
    }
}
