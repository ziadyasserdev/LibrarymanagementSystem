using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Dtos
{
    public class CreateLocationDto
    {
        public int Id { get; set; }
        public string Shelf { get; set; }
        public string Floor { get; set; }
        public string Section { get; set; }
        public int Capacity { get; set; }
        public int BranchId { get; set; }   
    }
}
