using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.SearchBooks
{
    public class SearchBooksQueryHandler : IRequestHandler<SearchBooksQuery, Result<PaginatedResult<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public SearchBooksQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<PaginatedResult<object>>> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
        {
         

            var query = unitOfWork.Books.Query()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .AsQueryable();

          
            if (!currentUserService.IsInRole("Admin"))
            {
                query = query.Where(b => b.IsActive && !b.IsDeleted);
            }
            else
            {
               
                query = request.Status switch
                {
                    BookStatusFilter.ActiveOnly => query.Where(b => b.IsActive && !b.IsDeleted),
                    BookStatusFilter.InactiveOnly => query.Where(b => !b.IsActive || b.IsDeleted),
                    _ => query
                };
            }

         
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(term) ||
                    b.ISBN.ToLower().Contains(term) ||
                    b.Author.Name.ToLower().Contains(term) ||
                    b.Category.Name.ToLower().Contains(term) ||
                    b.Publisher.Name.ToLower().Contains(term));
            }

        
            if (request.AuthorId.HasValue)
                query = query.Where(b => b.AuthorId == request.AuthorId.Value);

            if (request.CategoryId.HasValue)
                query = query.Where(b => b.CategoryId == request.CategoryId.Value);

            if (request.PublisherId.HasValue)
                query = query.Where(b => b.PublisherId == request.PublisherId.Value);

            if (request.MinPrice.HasValue)
                query = query.Where(b => b.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(b => b.Price <= request.MaxPrice.Value);

        
            query = request.SortBy switch
            {
                BookSortBy.Price => request.Desc ? query.OrderByDescending(b => b.Price)
                                                : query.OrderBy(b => b.Price),

                BookSortBy.PublishedYear => request.Desc ? query.OrderByDescending(b => b.PublishedYear)
                                                         : query.OrderBy(b => b.PublishedYear),

                BookSortBy.CreatedAt => request.Desc ? query.OrderByDescending(b => b.CreatedAt)
                                                     : query.OrderBy(b => b.CreatedAt),

                _ => request.Desc ? query.OrderByDescending(b => b.Title)
                                  : query.OrderBy(b => b.Title),
            };

        
            var totalCount = await query.CountAsync(cancellationToken);
         


          
            if (currentUserService.IsInRole("Admin"))
            {
                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(b => new BookAdminDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        ISBN = b.ISBN,
                        PublishedYear = b.PublishedYear,
                        Price = b.Price,
                        AuthorName = b.Author.Name,
                        CategoryName = b.Category.Name,
                        PublisherName = b.Publisher.Name,
                        IsActive = b.IsActive,
                        IsDeleted = b.IsDeleted,
                        CreatedAt=b.CreatedAt
                    })
                    .ToListAsync(cancellationToken);

               var paginatedResult = new PaginatedResult<object>(items, request.PageNumber, request.PageSize, totalCount);
                return Result<PaginatedResult<object>>.Success(paginatedResult);
            }
            else
            {
                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(b => new BookUserDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        ISBN = b.ISBN,
                        PublishedYear = b.PublishedYear,
                        Price = b.Price,
                        AuthorName = b.Author.Name,
                        CategoryName = b.Category.Name,
                        PublisherName = b.Publisher.Name,
                        CreatedAt=b.CreatedAt
                    })
                    .ToListAsync(cancellationToken);
                var paginatedResult = new PaginatedResult<object>(items, request.PageNumber, request.PageSize, totalCount);
                return Result<PaginatedResult<object>>.Success(paginatedResult);

            }
        
        }


        }
    }

