using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsActive { get; set; }=true;
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
