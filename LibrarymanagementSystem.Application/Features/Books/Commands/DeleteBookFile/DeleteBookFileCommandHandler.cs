using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using LibrarymanagementSystem.Application.Settings;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.DeleteBookFile
{
    public class DeleteBookFileCommandHandler : IRequestHandler<DeleteBookFileCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FileStorageSettings _settings;
        private readonly IFileService fileService;

        public DeleteBookFileCommandHandler(IUnitOfWork unitOfWork, IOptions<FileStorageSettings> settings, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this._settings = settings.Value;
            this.fileService = fileService;
        }
        public async Task<Result<int>> Handle(DeleteBookFileCommand request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Book not found.");
            if (string.IsNullOrEmpty(book.BookFileUrl))
                return Result<int>.Failure(ResultStatus.ValidationError, "No file associated with this book.");
            var removeResult = fileService.Remove(book.BookFileUrl);
            if (!removeResult.IsSuccess)
                return Result<int>.Failure(ResultStatus.Failure, removeResult.Error);
            book.BookFileUrl = null;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(book.Id);
        }
    }
}
