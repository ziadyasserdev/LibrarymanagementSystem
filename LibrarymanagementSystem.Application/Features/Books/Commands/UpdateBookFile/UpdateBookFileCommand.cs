using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace LibrarymanagementSystem.Application.Features.Books.Commands.UpdateBookFile
{
    public class UpdateBookFileCommand:IRequest<Result<FileDto>>
    {
        public int BookId { get; set; }
        public IFormFile NewFile { get; set; }
       
    }
}
