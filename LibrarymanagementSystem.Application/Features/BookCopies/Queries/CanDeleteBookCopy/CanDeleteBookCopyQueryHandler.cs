using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.CanDeleteBookCopy
{
    public class CanDeleteBookCopyQueryHandler : IRequestHandler<CanDeleteBookCopyQuery, Result<CanDeleteBookCopyDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CanDeleteBookCopyQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<CanDeleteBookCopyDto>> Handle(CanDeleteBookCopyQuery request, CancellationToken cancellationToken)
        {
            var bookCopy = await unitOfWork.BookCopies.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            var canDelete = new CanDeleteBookCopyDto
            {
                Id = request.Id
            };

            if (bookCopy == null)
            {
                canDelete.CanDelete = false;
                canDelete.Reason = "Book copy not found.";
                return Result<CanDeleteBookCopyDto>.Success(canDelete);
            }

            if (bookCopy.IsLost)
            {
                canDelete.CanDelete = false;
                canDelete.Reason = "Cannot delete a lost book copy.";
                return Result<CanDeleteBookCopyDto>.Success(canDelete);
            }

            if (bookCopy.Status == BookCopyStatus.Loaned)
            {
                canDelete.CanDelete = false;
                canDelete.Reason = "Cannot delete a loaned book copy.";
                return Result<CanDeleteBookCopyDto>.Success(canDelete);
            }

            if (bookCopy.IsDeleted)
            {
                canDelete.CanDelete = false;
                canDelete.Reason = "Book copy already deleted.";
                return Result<CanDeleteBookCopyDto>.Success(canDelete);
            }

            canDelete.CanDelete = true;
            canDelete.Reason = null;

            return Result<CanDeleteBookCopyDto>.Success(canDelete);
        }
    }
}
