using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Settings
{
    public  class LibraryPolicySetting
    {
        public  int MaxActiveLoansPerUser = 3;
        public  int MaxLoanDays = 3;
        public decimal FinePerDay { get; set; } = 5;
    }

}
