using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Dtos
{
    public class SpecificLocationStatisticsDto
    {
        public int Id { get; set; }
        public string Shelf { get; set; }
        public string Floor { get; set; }
        public string Section { get; set; }

        public int BranchId { get; set; }
        public string BranchName { get; set; }

        public int TotalBooks { get; set; }        
        public int AvailableBooks { get; set; }    
        public int BorrowedBooks { get; set; }     
        public int LostBooks { get; set; }        

        public int Capacity { get; set; }          
        public int OccupiedSlots { get; set; }    

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
