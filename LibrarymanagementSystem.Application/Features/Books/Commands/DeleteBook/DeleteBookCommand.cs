using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.DeleteBook
{
    public class DeleteBookCommand:IRequest<Result<int>>,IInvalidateCache
    {
        public int Id { get; set; }
        public DeleteBookCommand(int Id)
        {
            this.Id = Id;   
        }
        public string[] CacheKeysToInvalidate => new[] { CacheKeys.Books.AllBooks,
                                                       
                                                         CacheKeys.Books.BookById(Id)};
    }
}
