using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Dtos
{
    public class CreateBranchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public string Address { get; set; }
        public string City { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


