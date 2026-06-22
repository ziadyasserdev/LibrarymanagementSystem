using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.CanDeleteAuthor
{
    public class CanDeleteAuthorQuery:IRequest<Result<CanDeleteAuthorDto>>
    {
        public int AuthorId { get; set; }
        public CanDeleteAuthorQuery(int id) => AuthorId = id;
    }
}
