using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationCapacity
{
    public class GetLocationCapacityQuery : IRequest<Result<LocationCapacityDto>>

    {
        public int Id { get; set; }
        public GetLocationCapacityQuery(int id)
        {
            Id = id;

        }

    }

}