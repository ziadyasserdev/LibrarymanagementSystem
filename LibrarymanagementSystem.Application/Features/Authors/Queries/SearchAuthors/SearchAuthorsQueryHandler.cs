using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.SearchAuthors
{
    public class SearchAuthorsQueryHandler : IRequestHandler<SearchAuthorsQuery, Result<PaginatedResult<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public SearchAuthorsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<PaginatedResult<object>>> Handle(SearchAuthorsQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Authors.Query();

           
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(term) || x.Biography.ToLower().Contains(term));
            }

           
            if (currentUserService.IsInRole("Admin"))
            {
                if (request.IncludeDeleted.HasValue)
                    query = query.Where(x => x.IsDeleted == request.IncludeDeleted.Value);
            }
            else
            {
                query = query.Where(x => !x.IsDeleted); 
            }

          
            query = request.AuthorSort switch
            {
                AuthorSort.CreatedAt => request.Descending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                _ => request.Descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
            };

       
            var totalCount = await query.CountAsync(cancellationToken);
            var pagedQuery = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

           
            if (currentUserService.IsInRole("Admin"))
            {
                var data = await pagedQuery.Select(x => new AuthorSearchAdminDto
                {
                    AuthorId = x.AuthorId,
                    Name = x.Name,
                    Biography = x.Biography,
                    IsDeleted = x.IsDeleted,
                    CreatedAt= x.CreatedAt,
                }).ToListAsync(cancellationToken);

                return Result<PaginatedResult<object>>.Success(new PaginatedResult<object>(data, request.PageNumber, request.PageSize, totalCount));
            }
            else
            {
                var data = await pagedQuery.Select(x => new AuthorSearchDto
                {
                    AuthorId = x.AuthorId,
                    Name = x.Name,
                    Biography = x.Biography,
                    CreatedAt= x.CreatedAt
                }).ToListAsync(cancellationToken);

                return Result<PaginatedResult<object>>.Success(new PaginatedResult<object>(data, request.PageNumber, request.PageSize, totalCount));
            }
        }
    }
}

