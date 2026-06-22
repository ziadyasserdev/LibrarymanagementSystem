using FluentValidation;
using LibrarymanagementSystem.Application.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.CreateBookFile
{

    public class CreateBookFileCommandValidator : AbstractValidator<CreateBookFileCommand>
    {
        public CreateBookFileCommandValidator(IOptions<FileStorageSettings> fileSettings)
        {
            var settings = fileSettings.Value;

          
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be greater than zero.");

          
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.");

          
            RuleFor(x => x.File.Length)
                .GreaterThan(0)
                .WithMessage("File cannot be empty.")
                .When(x => x.File != null);

          
            //RuleFor(x => x.File)
            //    .Must(file =>
            //    {
            //        if (file == null) return true; 
            //        var maxSizeMB = settings.FileSizeInMB;
            //        return file.Length <= maxSizeMB * 1024 * 1024;
            //    })
            //    .WithMessage($"File size cannot exceed {settings.FileSizeInMB} MB.")
            //    .When(x => x.File != null);

          
            //RuleFor(x => x.File)
            //    .Must(file =>
            //    {
            //        if (file == null) return true;
            //        var ext = Path.GetExtension(file.FileName).ToLower();
            //        return settings.FileExtentionAllowed.Select(e => e.ToLower()).Contains(ext);
            //    })
            //    .WithMessage($"Only the following file types are allowed: {string.Join(", ", settings.FileExtentionAllowed)}")
            //    .When(x => x.File != null);
        }
    }
}
