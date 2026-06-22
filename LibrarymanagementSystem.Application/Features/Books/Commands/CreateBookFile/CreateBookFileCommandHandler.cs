using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using LibrarymanagementSystem.Application.Settings;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.CreateBookFile
{
    public class CreateBookFileCommandHandler : IRequestHandler<CreateBookFileCommand, Result<FileDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IFileService fileService;
        private readonly FileStorageSettings _settings;

        public CreateBookFileCommandHandler(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IOptions<FileStorageSettings> settings,IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
            this.fileService = fileService;
            this._settings = settings.Value;
        }
        public async Task<Result<FileDto>> Handle(CreateBookFileCommand request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book == null)
                return Result<FileDto>.Failure(ResultStatus.NotFound, "Book not found.");
            var result = await fileService.UploadFileAsync(request.File);
            if(!result.IsSuccess)
                return Result<FileDto>.Failure(result.Status, result.Error);
            book.BookFileUrl= result.Value!.Url;
            await unitOfWork.SaveAsync();
            return Result<FileDto>.Success(result.Value!);
        }







        


    }
}
