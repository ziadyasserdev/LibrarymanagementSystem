using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.BulkCreatePublishers
{
    public class BulkCreatePublishersCommandHandler : IRequestHandler<BulkCreatePublishersCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkCreatePublishersCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkCreatePublishersCommand request, CancellationToken cancellationToken)
        {
            if (request.Publishers == null || !request.Publishers.Any())
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "No publishers provided");

            var names = request.Publishers
               .Select(x => x.Name.Trim())
               .ToList();

            var emails = request.Publishers
                .Select(x => x.Email.Trim())
                .ToList();

            var websites = request.Publishers
                .Select(x => x.Website.Trim())
                .ToList();


            var existingPublishers = await unitOfWork.Publishers.Query()

               .Where(x => !x.IsDeleted)

               .Where(x =>
                   names.Contains(x.Name) ||
                   emails.Contains(x.Email) ||
                   websites.Contains(x.Website))

               .ToListAsync(cancellationToken);



            var duplicateNamesInDb =
                existingPublishers
                .Select(x => x.Name)
                .Intersect(names)
                .ToList();


            if (duplicateNamesInDb.Any())
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Duplicate Names in database: {string.Join(",", duplicateNamesInDb)}");



            var duplicateEmailsInDb =
                existingPublishers
                .Select(x => x.Email)
                .Intersect(emails)
                .ToList();


            if (duplicateEmailsInDb.Any())
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Duplicate Emails in database: {string.Join(",", duplicateEmailsInDb)}");



            var duplicateWebsitesInDb =
                existingPublishers
                .Select(x => x.Website)
                .Intersect(websites)
                .ToList();


            if (duplicateWebsitesInDb.Any())
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Duplicate Websites in database: {string.Join(",", duplicateWebsitesInDb)}");



         

            var publishers = request.Publishers
                .Select(dto => new Publisher
                {
                    Name = dto.Name.Trim(),
                    Email = dto.Email.Trim(),
                    Phone = dto.Phone,
                    Website = dto.Website.Trim(),

                    Address = dto.Address,
                    City = dto.City,
                    Country = dto.Country,
                    Description = dto.Description,

                    IsActive = true,
                    IsDeleted = false,

                    CreatedAt = DateTime.Now,

                    CreatedBy =
                        currentUserService.UserName
                        ?? "System"

                }).ToList();



          

            await unitOfWork.Publishers
                .AddRange(publishers);


            await unitOfWork
                .SaveAsync();



          

            return Result<string>.Success(

                $"{publishers.Count} publishers created successfully"

            );



        }
    }
}
