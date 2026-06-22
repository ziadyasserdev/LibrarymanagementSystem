using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Dtos
{
    public class CreateReviewDto
    {
        public int BookId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
