using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Dtos
{
    public class BookRatingSummaryDto
    {
        public int BookId { get; set; }

        public double AverageRating { get; set; }

        public int TotalReviews { get; set; }
    }
}
