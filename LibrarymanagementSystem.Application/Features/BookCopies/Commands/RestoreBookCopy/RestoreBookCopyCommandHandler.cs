using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.RestoreBookCopy
{
    public class RestoreBookCopyCommandHandler : IRequestHandler<RestoreBookCopyCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public RestoreBookCopyCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(RestoreBookCopyCommand request, CancellationToken cancellationToken)
        {
            var bookCopy = await unitOfWork.BookCopies.Query()
     .IgnoreQueryFilters()
     .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (bookCopy == null)
                return Result<int>.Failure(ResultStatus.NotFound, "BookCopy not found");

            if (!bookCopy.IsDeleted)
                return Result<int>.Failure(ResultStatus.Failure, "BookCopy is already active");

            if (bookCopy.IsLost)
                return Result<int>.Failure(ResultStatus.Conflict, "Cannot restore a lost book copy");

            if (bookCopy.Status == BookCopyStatus.Loaned)
                return Result<int>.Failure(ResultStatus.Conflict, "Cannot restore a loaned book copy");

            var checkDuplicate = await unitOfWork.BookCopies.Query()
                .AnyAsync(x => x.Barcode == bookCopy.Barcode
                    && !x.IsDeleted && x.Id != bookCopy.Id, cancellationToken);

            if (checkDuplicate)
                return Result<int>.Failure(ResultStatus.Conflict, "A book copy with the same barcode already exists");

            bookCopy.IsDeleted = false;
            bookCopy.DeletedAt = null;
            bookCopy.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(bookCopy.Id);
        }
    }
}
