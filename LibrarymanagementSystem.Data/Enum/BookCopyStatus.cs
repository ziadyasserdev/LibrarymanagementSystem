using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Enum
{
    public enum BookCopyStatus
    {
        Available = 1,
        Loaned = 2,
        Reserved = 3,
        Lost = 4,
        Damaged = 5,
        Maintenance = 6
    }
}
