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

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.CreatePublisher
{
    public class CreatePublisherCommandHandler : IRequestHandler<CreatePublisherCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreatePublisherCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
        {
            var publisherExist = await unitOfWork.Publishers
                .Query()
                .FirstOrDefaultAsync(c => c.Name
                == request.Name && !c.IsDeleted);
          if (publisherExist != null)
           return Result<int>.Failure(ResultStatus.Failure, "Publisher name already exist");

          var duplicateWebsite=await unitOfWork.Publishers
                .Query().AnyAsync(x=>x.Website== request.Website && !x.IsDeleted);
            if (duplicateWebsite)
                return Result<int>.Failure(ResultStatus.Failure, "Publisher website already exist");

            var duplicateEmail = await unitOfWork.Publishers
                .Query().AnyAsync(x => x.Email == request.Email && !x.IsDeleted);
            if (duplicateEmail)
                return Result<int>.Failure(ResultStatus.Failure, "Publisher email already exist");

            var publisher = new Publisher
            {
                Name = request.Name,
                Email = request.Email,
                Phone=request.Phone,
                Website=request.Website,
                CreatedAt = DateTime.Now,
                City = request.City,
                Country = request.Country,
                Address = request.Address,
                Description= request.Description,
                CreatedBy=currentUserService.UserName??"Default Admin",
             
            };

            await unitOfWork.Publishers.AddAsync(publisher);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(publisher.Id);
        }
    }
}
