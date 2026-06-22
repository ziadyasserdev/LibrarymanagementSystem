using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Dtos
{
    public class PublisherStatisticsDto
    {
        public int TotalBooks { get; set; }

        public int TotalBorrowedBooks { get; set; }

        public double AverageBookRating { get; set; }

        public int TotalReviews { get; set; }
    }
}
