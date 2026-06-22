using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Dtos
{
    public class FineReportDto
    {
        public decimal TotalOverdueFines { get; set; }      
        public decimal TotalPaidFines { get; set; }         
    
        public int LateFinesCount { get; set; }             
        public int LostFinesCount { get; set; }           
    }
}
