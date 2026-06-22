using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class ReviewReport
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public string Reason { get; set; }
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
        public Review Review { get; set; }
    }
}
