using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Dtos
{
    public class PublisherReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<int> BookIds { get; set; } = new();
    }

}
