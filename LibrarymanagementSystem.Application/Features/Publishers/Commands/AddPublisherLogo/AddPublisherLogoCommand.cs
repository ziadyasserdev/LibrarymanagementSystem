using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.AddPublisherLogo
{
    public class AddPublisherLogoCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public IFormFile formFile { get; set; }
    }
}
