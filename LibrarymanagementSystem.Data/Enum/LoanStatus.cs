using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Enum
{
    public enum LoanStatus
    {
        Active = 1,              // الكتاب مع المستخدم
        Returned = 2,            // رجع الكتاب
        PendingFinePayment = 3,  // في غرامة ولسه مدفعتش
        Closed = 4,  
        Lost= 5,
    }

}
