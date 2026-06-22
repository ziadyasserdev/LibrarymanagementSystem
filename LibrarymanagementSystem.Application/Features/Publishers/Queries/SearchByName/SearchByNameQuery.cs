using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.SearchByName
{
    public class SearchByNameQuery:IRequest<Result<List<PublisherReadDto>>>
    {
        public string? name;
        public string? country;
        public PublisherStatusFilter publisherStatusFilter = PublisherStatusFilter.Active;
        public SearchByNameQuery(string n, PublisherStatusFilter publisherStatusFilter, string country)
        {
            this.name = n;
            this.publisherStatusFilter = publisherStatusFilter;
            this.country = country;
        }

    }
}
