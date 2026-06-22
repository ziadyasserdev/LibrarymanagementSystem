using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public DateTime MembershipDate { get; set; } = DateTime.Now;
        public Gender Gender { get; set; }
        public ICollection<LoanBook> LoanBooks { get; set; } = new List<LoanBook>();
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<RefreshToken> RefreshTokens { get; set; }  
        public bool IsDeleted { get; set; }=false;
        public string? ProfileImage { get; set; }

        public bool IsLocked { get; set; } = false;
        public ICollection<Reservation> Reservations { get; set; }
      = new List<Reservation>();
        public DateTime? LockedAt { get; set; }   
        public string? LockedByAdminId { get; set; }

    }
}
