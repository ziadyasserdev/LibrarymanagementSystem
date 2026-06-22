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

namespace LibrarymanagementSystem.Application.Features.Books.Commands.UpdateBookFile
{
    public class UpdateBookFileCommandHandler : IRequestHandler<UpdateBookFileCommand, Result<FileDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly FileStorageSettings _settings;
        private readonly IFileService fileService;

        public UpdateBookFileCommandHandler(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IOptions<FileStorageSettings> settings, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
            this._settings = settings.Value;
            this.fileService = fileService;
        }
        public async Task<Result<FileDto>> Handle(UpdateBookFileCommand request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book == null)
                return Result<FileDto>.Failure(ResultStatus.NotFound, "Book not found.");
            if (!string.IsNullOrWhiteSpace(book.BookFileUrl))
            {
                fileService.Remove(book.BookFileUrl);
            }
            var result = await fileService.UploadFileAsync(request.NewFile);
            if (!result.IsSuccess)
                return Result<FileDto>.Failure(result.Status, result.Error);
            book.BookFileUrl = result.Value!.Url;
            await unitOfWork.SaveAsync();
            return Result<FileDto>.Success(result.Value!);
        }
    }
}
