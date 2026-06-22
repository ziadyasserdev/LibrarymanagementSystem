using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Dtos
{
    public class CanDeleteAuthorDto
    {
        public int AuthorId { get; set; }
        public bool CanDelete { get; set; }
        public string Reason { get; set; } = null!;
    }
}
