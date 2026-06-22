using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.UpdatePublisher
{
    public class UpdatePublisherCommandHandler : IRequestHandler<UpdatePublisherCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdatePublisherCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = await unitOfWork.Publishers.Query()
                .FirstOrDefaultAsync(c=>c.Id == request.Id && !c.IsDeleted);
            if (publisher == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Publisher not found");


            var checkDuplicate=await unitOfWork.Publishers.Query()
                .AnyAsync(x=>x.Name == request.Name && x.Id!=request.Id && !x.IsDeleted);

            if (checkDuplicate)
                return Result<int>.Failure(ResultStatus.Failure, "Publisher already exist");


            var duplicateWebsite = await unitOfWork.Publishers
               .Query().AnyAsync(x => x.Website == request.Website && !x.IsDeleted && x.Id!=request.Id);
            if (duplicateWebsite)
                return Result<int>.Failure(ResultStatus.Failure, "Publisher website already exist");

            var duplicateEmail = await unitOfWork.Publishers
                .Query().AnyAsync(x => x.Email == request.Email && !x.IsDeleted && x.Id != request.Id);

            if (duplicateEmail)
                return Result<int>.Failure(ResultStatus.Failure, "Publisher email already exist");

            publisher.Name=request.Name ?? publisher.Name;
            publisher.Email=request.Email ?? publisher.Email;
            publisher.Website=request.Website ?? publisher.Website;  
            publisher.Phone=request.Phone ?? publisher.Phone;
            publisher.UpdatedAt = DateTime.Now;
            publisher.Address=request.Address ?? publisher.Address;
            publisher.City = request.City ?? publisher.City;
            publisher.Country = request.Country ?? publisher.Country;
            publisher.UpdatedBy = currentUserService.UserName ?? "Default Admin";
            publisher.Description = request.Description ?? publisher.Description;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(publisher.Id);
        }
    }
}
