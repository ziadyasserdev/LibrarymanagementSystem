using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorByCategoryName;
using LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorsByCategoryId;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByAuthorName;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByCategoryId;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByCategoryName;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksWithPagination;
using LibrarymanagementSystem.Application.Features.Categories.Commands.BulkRestoreCategory;
using LibrarymanagementSystem.Application.Features.Categories.Commands.BulkSoftDeleteCategory;
using LibrarymanagementSystem.Application.Features.Categories.Commands.CreateCategory;
using LibrarymanagementSystem.Application.Features.Categories.Commands.DeleteCategory;
using LibrarymanagementSystem.Application.Features.Categories.Commands.RestoreCategory;
using LibrarymanagementSystem.Application.Features.Categories.Commands.UpdateCategory;
using LibrarymanagementSystem.Application.Features.Categories.Queries.GetAllCategories;
using LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesStatistics;
using LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesWithPagination;
using LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoryById;
using LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoryCount;
using LibrarymanagementSystem.Application.Features.Categories.Queries.SearchCategory;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Manage categories: create, update, delete, list, pagination, and get books/authors by category.")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // ===================== GET =====================
        [HttpGet]
        [SwaggerOperation(Summary = "Get all categories", Description = "Retrieve all categories.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllCategoryQuery());
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get category by ID", Description = "Retrieve a specific category by its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetCategoryByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("{categoryName}/books")]
        [SwaggerOperation(Summary = "Get books by category name", Description = "Retrieve all books that belong to a specific category.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooksByCategoryName(string categoryName)
        {
            var result = await mediator.Send(new GetBooksByCategoryNameQuery(categoryName.ToLower()));
            return result.ToActionResult();
        }

        [HttpGet("{categoryName}/authors")]
        [SwaggerOperation(Summary = "Get authors by category name", Description = "Retrieve all authors who have books in a specific category.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthorsByCategoryName(string categoryName)
        {
            var result = await mediator.Send(new GetAuthorByCategoryNameQuery(categoryName.ToLower()));
            return result.ToActionResult();
        }

        [HttpGet("{categoryId:int}/authors")]
        [SwaggerOperation(
       Summary = "Get authors by category Id",
       Description = "Retrieve all authors that belong to a specific category using the category Id."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAuthorsByCategoryId(int categoryId)
        {
            var result = await mediator.Send(new GetAuthorsByCategoryIdQuery(categoryId));
            return result.ToActionResult();
        }

        [HttpGet("{categoryId:int}/books")]
        [SwaggerOperation(
       Summary = "Get books by category Id",
       Description = "Retrieve all books that belong to a specific category using the category Id."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBooksByCategoryId(int categoryId)
        {
            var result = await mediator.Send(new GetBooksByCategoryIdQuery(categoryId));
            return result.ToActionResult();
        }

        [HttpPost("{id:int}/restore")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
      Summary = "Restore category",
      Description = "Restores a soft-deleted category by its Id."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore(int id)
        {
          

            var result = await mediator.Send(new RestoreCategoryCommand { Id=id});
            return result.ToActionResult();
        }

        [HttpGet("pagination")]
        [SwaggerOperation(Summary = "Get categories with pagination", Description = "Retrieve categories using pagination parameters 'pageNumber' and 'pageSize'.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoriesWithPagination([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await mediator.Send(new GetCategoriesWithPaginationQuery(pageNumber, pageSize));
            return result.ToActionResult();
        }

        // ===================== POST =====================
        [HttpPost]
        [SwaggerOperation(Summary = "Create category", Description = "Add a new category to the system.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        // ===================== PUT =====================
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update category", Description = "Update an existing category using its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        // ===================== DELETE =====================
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete category", Description = "Delete a category by its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await mediator.Send(new DeleteCategoryCommand(id));
            return result.ToActionResult();
        }
        [HttpGet("count")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Get categories count",
            Description = "Returns the total number of categories. Optionally filter by deletion status."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCount([FromQuery] bool? isDeleted)
        {
            var result = await mediator.Send(
                new GetCategoryCountQuery { IsDeleted=isDeleted}
            );

            return result.ToActionResult();
        }

        [HttpPatch("bulk/restore")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
       Summary = "Bulk restore categories",
       Description = "Restores multiple soft-deleted categories using a list of category IDs."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RestoreBulk([FromBody] List<int> categoryIds)
        {
          

            var result = await mediator.Send(new BulkRestoreCategoryCommand(categoryIds));
            return result.ToActionResult();
        }

        [HttpPatch("bulk/softdelete")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
     Summary = "Bulk soft delete categories",
     Description = "Soft deletes multiple categories using a list of category IDs."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkSoftDelete([FromBody] List<int> categoryIds)
        {
          

            var result = await mediator.Send(new BulkSoftDeleteCategoryCommand(categoryIds));
            return result.ToActionResult();
        }



        [HttpGet("statistics")]
      //  [Authorize(Roles = "Admin")]
        [SwaggerOperation(
      Summary = "Get categories statistics",
      Description = "Retrieves statistics for categories such as total count, active count, and deleted count."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoriesStatistics()
        {
            var result = await mediator.Send(new GetCategoriesStatisticsQuery());
            return result.ToActionResult();
        }



        [HttpGet("search")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
     Summary = "Search categories with pagination and sorting",
     Description = "Searches categories by name. Supports pagination, sorting, and optional inclusion of deleted categories."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchCategories(
     [FromQuery] string? searchTerm,
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] CategorySort categorySort = CategorySort.ByName,
     [FromQuery] bool descending = false,
     [FromQuery] bool? includeDeleted = null)
        {
           
            var query = new SearchCategoryQuery
            {
                SearchTerm = searchTerm,
                PageNumber = pageNumber,
                PageSize = pageSize,
                categorySort = categorySort,  
                Descending = descending,
                IncludeDeleted = includeDeleted
            };

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
    }

}
