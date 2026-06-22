using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.ActivateBook
{
    public class ActivateBookCommandHandler : IRequestHandler<ActivateBookCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public ActivateBookCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(ActivateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
            if(book is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Book not found or deleted");
            if(book.IsActive)
                return Result<int>.Failure(ResultStatus.Conflict, "Book is already active");
            book.IsActive = true;
            book.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(book.Id);
        }
    }
}
