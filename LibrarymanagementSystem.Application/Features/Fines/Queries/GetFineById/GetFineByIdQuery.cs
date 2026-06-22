using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineById
{
    public class GetFineByIdQuery:IRequest<Result<FineDto>>
    {
        public int Id { get; set; }
        public GetFineByIdQuery(int id)
        {
            Id = id;
        }
    }
}
