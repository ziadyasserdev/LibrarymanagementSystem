using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using LibrarymanagementSystem.Application.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.DownloadBookFile
{
    public class DownloadBookFileQueryHandler : IRequestHandler<DownloadBookFileQuery, Result<BookFileDownloadDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FileStorageSettings _settings;
        private readonly IFileService fileService;

        public DownloadBookFileQueryHandler(IUnitOfWork unitOfWork, IOptions<FileStorageSettings> settings, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this._settings = settings.Value;
            this.fileService = fileService;
        }
        public async Task<Result<BookFileDownloadDto>> Handle(DownloadBookFileQuery request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book == null)
                return Result<BookFileDownloadDto>.Failure(ResultStatus.NotFound, "Book not found.");
            if (string.IsNullOrEmpty(book.BookFileUrl))
                return Result<BookFileDownloadDto>.Failure(ResultStatus.ValidationError, "No file associated with this book.");
            var fileBytes=await fileService.DownloadFileAsync(book.BookFileUrl);
            var dto = new BookFileDownloadDto
            {
                FileName = book.BookFileUrl,
                ContentType = "application/pdf", 
                Content = fileBytes.Value!
            };
            return Result<BookFileDownloadDto>.Success(dto);
        }
    }
}
