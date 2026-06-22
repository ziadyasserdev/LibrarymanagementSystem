using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Dtos
{
    public class DeletedPublisherDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string? DeletedBy { get; set; }
    }
}
