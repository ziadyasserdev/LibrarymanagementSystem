using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.ApplicationUsers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Queries.CheckEmailExist
{
    public class CheckEmailExistQuery:IRequest<Result<ApplicationUserDto>>
    {
        public string Email { get; set; }
        public CheckEmailExistQuery(string e)
        {
            Email= e;   
        }
    }
}
