using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Dtos
{
    public class AuthorSearchDto
    {
        public int AuthorId { get; set; }             
        public string Name { get; set; } = string.Empty;    
        public string Biography { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}
