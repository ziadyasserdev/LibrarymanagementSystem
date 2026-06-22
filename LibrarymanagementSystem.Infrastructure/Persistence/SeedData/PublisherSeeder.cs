using LibrarymanagementSystem.Data.Models;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Persistence.SeedData
{
    public static class PublisherSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            
            if (await context.Publishers.AnyAsync())
                return;

            var publishers = new List<Publisher>
            {
                new Publisher
                {
                    Name = "Penguin Random House",
                    Email = "contact@penguinrandomhouse.com",
                    Phone = "+1-212-782-9000",
                    Website = "https://www.penguinrandomhouse.com",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                },

                new Publisher
                {
                    Name = "HarperCollins",
                    Email = "info@harpercollins.com",
                    Phone = "+1-212-207-7000",
                    Website = "https://www.harpercollins.com",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                },

                new Publisher
                {
                    Name = "Simon & Schuster",
                    Email = "contact@simonandschuster.com",
                    Phone = "+1-212-698-7000",
                    Website = "https://www.simonandschuster.com",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                }
            };

            await context.Publishers.AddRangeAsync(publishers);
            await context.SaveChangesAsync();
        }
    }

}
