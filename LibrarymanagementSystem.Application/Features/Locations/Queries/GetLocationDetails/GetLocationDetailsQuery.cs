using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationDetails
{
    public class GetLocationDetailsQuery:IRequest<Result<LocationDetailsDto>>
    {
        public int Id { get; set; }

        public GetLocationDetailsQuery(int id)
        {
            Id = id;
        }
    }
}
