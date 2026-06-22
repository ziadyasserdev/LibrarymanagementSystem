using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Dtos
{
    public class AuthResponseDto
    {
        public AuthTokenDto Auth { get; set; } = new();
        public UserResponseDto User { get; set; } = new();
    }

}
