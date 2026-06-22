using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reservations.Dtos
{
    public class AdminReservationDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public string BookTitle { get; set; }

        public int QueuePosition { get; set; }
        public string Status { get; set; }

        public DateTime ReservationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
