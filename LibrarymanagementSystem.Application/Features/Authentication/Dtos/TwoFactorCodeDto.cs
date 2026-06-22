using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Dtos
{
    public class TwoFactorCodeDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
