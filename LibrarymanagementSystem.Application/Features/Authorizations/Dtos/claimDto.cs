using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Dtos
{
    public class claimDto
    {
        public string claimType { get; set; }
        public string claimValue { get; set; }
        public string newClaimValue { get; set; }
    }
}
