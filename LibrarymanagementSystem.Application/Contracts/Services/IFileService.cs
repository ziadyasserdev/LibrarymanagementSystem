using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Contracts.Services
{
    public interface IFileService
    {
        Task<Result<FileDto>> UploadImageAsync(IFormFile file);
        Task<Result<FileDto>> UploadVideoAsync(IFormFile file);
        Task<Result<FileDto>> UploadFileAsync(IFormFile file);
        Result<string> Remove(string url);
        Task<Result<byte[]>> DownloadFileAsync(string url);
    }
}
