using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.CreateBook
{
    public class CreateBookCommand:IRequest<Result<BookDto>>,IInvalidateCache
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int PublishedYear { get; set; }
        public int NumberOfPages { get; set; }
        public string Language { get; set; } = null!;
        public int PublisherId { get; set; }        
           

        public string Edition { get; set; } = null!;
      
        public decimal Price { get; set; }
       
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        public string[] CacheKeysToInvalidate => new[] { CacheKeys.Books.AllBooks }
                                                        ;
    }
}
