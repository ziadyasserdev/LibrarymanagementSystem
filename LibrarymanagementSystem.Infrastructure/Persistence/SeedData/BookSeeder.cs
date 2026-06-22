using LibrarymanagementSystem.Data.Models;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Persistence.SeedData
{
    //public static class BookSeeder
    //{
    //    public static async Task SeedAsync(ApplicationDbContext context)
    //    {
    //        if (!context.Books.Any())
    //        {
    //            var author = context.Authors.First();
    //            var category = context.Categories.First();

    //            context.Books.Add(new Book
    //            {
    //                Title = "Clean Code",
    //                Description = "A handbook of agile software craftsmanship.",
    //                ISBN = "9780132350884",
    //                PublishedYear = 2008,
    //                NumberOfPages = 464,
    //                Language = "English",
    //                Publisher = "Prentice Hall",
    //                Edition = "1st Edition",
    //                Stock = 5,
    //                Price = 450,
    //                Location = "Shelf A3",
    //                BookFileUrl = null, // أو حط لينك لو عندك ملف PDF

    //                AuthorId = author.AuthorId,
    //                CategoryId = category.CategoryId
    //            });
    //            await context.SaveChangesAsync();
    //        }
    //    }
    //}



    public static class BookSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Books.Any())
            {

                var publisher1 = context.Publishers.FirstOrDefault() ?? throw new Exception("No Publisher found.");
                var author1 = context.Authors.FirstOrDefault() ?? throw new Exception("No Author found.");
                var category1 = context.Categories.FirstOrDefault() ?? throw new Exception("No Category found.");
                var location1 = context.Locations.FirstOrDefault() ?? throw new Exception("No Location found.");

                context.Books.AddRange(
                    new Book
                    {
                        Title = "The Great Gatsby",
                        Description = "Classic novel by F. Scott Fitzgerald",
                        ISBN = "9780743273565",
                        PublishedYear = 1925,
                        NumberOfPages = 218,
                        Language = "English",
                        Edition = "1st",
                       
                        Price = 15.99M,
                        BookFileUrl = null,
                        PublisherId = publisher1.Id,
                        Publisher = publisher1,
                        AuthorId = author1.AuthorId,
                        Author = author1,
                        CategoryId = category1.CategoryId,
                        Category = category1,
                       
                    },
                    new Book
                    {
                        Title = "A Brief History of Time",
                        Description = "Science book by Stephen Hawking",
                        ISBN = "9780553380163",
                        PublishedYear = 1988,
                        NumberOfPages = 212,
                        Language = "English",
                        Edition = "1st",
                       
                        Price = 20.50M,
                        BookFileUrl = null,
                        PublisherId = publisher1.Id,
                        Publisher = publisher1,
                        AuthorId = author1.AuthorId,
                        Author = author1,
                        CategoryId = category1.CategoryId,
                        Category = category1,
                      
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
