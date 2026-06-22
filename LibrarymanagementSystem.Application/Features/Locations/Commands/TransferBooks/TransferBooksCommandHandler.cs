using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.TransferBooks
{
    public class TransferBooksCommandHandler : IRequestHandler<TransferBooksCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public TransferBooksCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(TransferBooksCommand request, CancellationToken cancellationToken)
        {



            var fromLocation = await unitOfWork.Locations.Query()
              .FirstOrDefaultAsync(l => l.Id == request.FromLocationId
              && !l.IsDeleted 
              && l.IsActive
              , cancellationToken);

            if (fromLocation == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Source location not found.");

            var toLocation = await unitOfWork.Locations.Query()
                .FirstOrDefaultAsync(l => l.Id == request.ToLocationId
                  && !l.IsDeleted
              && l.IsActive
                , cancellationToken);

            if (toLocation == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Destination location not found.");

            if (request.FromLocationId == request.ToLocationId)
                return Result<string>.Failure(ResultStatus.Conflict, "Source and destination locations are the same.");

         
            var bookCopies = await unitOfWork.BookCopies.Query()
                .Where(bc => request.BookCopiesId.Contains(bc.Id) && bc.LocationId == request.FromLocationId)
                .ToListAsync(cancellationToken);

            if (!bookCopies.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No book copies found to transfer from the source location.");

            var toLocationCopiesCount = await unitOfWork.BookCopies.Query()
                .CountAsync(bc => bc.LocationId == request.ToLocationId, cancellationToken);

            if (toLocationCopiesCount + bookCopies.Count > toLocation.Capacity)
                return Result<string>.Failure(ResultStatus.Conflict,
                    $"Cannot transfer {bookCopies.Count} books. Destination  will exceed its capacity ({toLocation.Capacity}).");

       
            foreach (var copy in bookCopies)
            {
                copy.LocationId = request.ToLocationId;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success($"{bookCopies.Count} book copies successfully transferred ");


        }
    }
}
