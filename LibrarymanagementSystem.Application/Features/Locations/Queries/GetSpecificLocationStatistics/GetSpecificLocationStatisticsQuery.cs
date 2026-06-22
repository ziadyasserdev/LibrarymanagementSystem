using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetSpecificLocationStatistics
{
    public class GetSpecificLocationStatisticsQuery:IRequest<Result<SpecificLocationStatisticsDto>>
    {
        public int LocationId { get; set; }
        public GetSpecificLocationStatisticsQuery(int id)
        {
            LocationId=id;
        }
    }
}
