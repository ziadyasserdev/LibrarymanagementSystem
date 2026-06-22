using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetAllAvailableBooks
{
    //public class GetAllAvailableBooksQueryHandler : IRequestHandler<GetAllAvailableBooksQuery, Result<List<BookDto>>>
    //{
    //    private readonly IUnitOfWork unitOfWork;

    //    public GetAllAvailableBooksQueryHandler(IUnitOfWork unitOfWork)
    //    {
    //        this.unitOfWork = unitOfWork;
    //    }
    //    public async Task<Result<List<BookDto>>> Handle(GetAllAvailableBooksQuery request, CancellationToken cancellationToken)
    //    {
    //        var books=await unitOfWork.Books.GetAllAvailableBooksAsync();
    //        if(books==null)
    //            return Result<List<BookDto>>.Failure(ResultStatus.NotFound, "No available books found");
    //        var booksDto = books.Select(x => new BookDto
    //        {
    //            Id = x.Id,
    //            Title = x.Title,
    //            Description = x.Description,
    //            ISBN = x.ISBN,
    //            PublishedYear = x.PublishedYear,
    //            NumberOfPages = x.NumberOfPages,
    //            Language = x.Language,
    //            Publisher = x.publisher.Name,
    //            Edition = x.Edition,
    //            Stock = x.Stock,
    //            Price = x.Price,
    //            Location = new BookLocation
    //            {
    //                Floor = x.Location.Floor,
    //                Section = x.Location.Section,
    //                Shelf = x.Location.Shelf,
    //            },
    //            AuthorName = x.Author.Name,
    //            CategoryName = x.Category.Name,
    //            BookFileUrl = x.BookFileUrl,
    //            NumberOfBorrowRecords = x.LoanBooks.Count()


    //        }).ToList();
    //        return Result<List<BookDto>>.Success(booksDto);
    //    }
    //}
}
