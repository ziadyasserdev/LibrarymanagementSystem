using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Dtos
{
    public class PublisherCountDto
    {
        public int TotalPublishers { get; set; }

        public int ActivePublishers { get; set; }

        public int InactivePublishers { get; set; }

        public int DeletedPublishers { get; set; }
    }
}
