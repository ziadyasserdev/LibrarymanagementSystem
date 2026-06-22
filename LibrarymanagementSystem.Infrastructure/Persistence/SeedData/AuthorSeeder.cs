using LibrarymanagementSystem.Data.Models;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Persistence.SeedData
{

    public static class AuthorSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Authors.Any())
            {
                context.Authors.AddRange(
                    new Author { Name = "Author 1", Biography = "Bio 1" },
                    new Author { Name = "Author 2", Biography = "Bio 2" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
