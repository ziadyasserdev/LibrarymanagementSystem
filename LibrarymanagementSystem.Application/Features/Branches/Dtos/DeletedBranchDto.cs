using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Dtos
{
    public class DeletedBranchDto
    {
        public int Id { get; set; }            
        public string Name { get; set; }      
        public string Address { get; set; }    
        public string City { get; set; }       
        public DateTime? DeletedAt { get; set; } 
    }
}
