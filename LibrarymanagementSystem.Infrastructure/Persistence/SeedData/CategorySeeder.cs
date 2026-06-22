using LibrarymanagementSystem.Data.Models;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Persistence.SeedData
{
    public static class CategorySeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Fiction", Description = "Fiction Books" },
                    new Category { Name = "Science", Description = "Science Books" },
                    new Category { Name = "History", Description = "History Books" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
