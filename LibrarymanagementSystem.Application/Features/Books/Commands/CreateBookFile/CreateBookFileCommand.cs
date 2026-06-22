using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.CreateBookFile
{
    public class CreateBookFileCommand:IRequest<Result<FileDto>>
    {
            public int BookId { get; set; }
            public IFormFile File { get; set; }
          
        
    }
}
