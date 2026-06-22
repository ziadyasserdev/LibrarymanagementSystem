using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.SearchBooks
{
    public class SearchBooksQueryValidator : AbstractValidator<SearchBooksQuery>
    {
        public SearchBooksQueryValidator()
        {
        
            RuleFor(q => q.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be greater than or equal to 1.");

            RuleFor(q => q.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must be between 1 and 100.");

         
            RuleFor(q => q)
                .Must(q => !q.MinPrice.HasValue || !q.MaxPrice.HasValue || q.MinPrice.Value <= q.MaxPrice.Value)
                .WithMessage("MinPrice cannot be greater than MaxPrice.");

       
            RuleFor(q => q.Status)
                .IsInEnum()
                .WithMessage("Invalid Status value.");

            RuleFor(q => q.SortBy)
                .IsInEnum()
                .WithMessage("Invalid SortBy value.");

         
            RuleFor(q => q.SearchTerm)
                .MaximumLength(200)
                .WithMessage("Search term must not exceed 200 characters.");
        }
    }
}
