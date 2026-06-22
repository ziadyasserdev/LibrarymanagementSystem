using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Data.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;

        public ReservationStatus Status { get; set; }

        public int QueuePosition { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? FulfilledAt { get; set; }

        public DateTime? CancelledAt { get; set; }
    }
}
