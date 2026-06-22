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
    public static class BranchSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Branches.AnyAsync())
                return;

            var branches = new List<Branch>
            {
                new Branch
                {
                    Name = "Cairo Main Branch",
                    Code = "CAI-01",
                    Address = "Downtown Cairo",
                    City = "Cairo",
                    PhoneNumber = "01000000001",
                    Email = "cairo@library.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Branch
                {
                    Name = "Alexandria Branch",
                    Code = "ALX-01",
                    Address = "Smouha District",
                    City = "Alexandria",
                    PhoneNumber = "01000000002",
                    Email = "alex@library.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Branch
                {
                    Name = "Giza Branch",
                    Code = "GIZ-01",
                    Address = "Dokki Area",
                    City = "Giza",
                    PhoneNumber = "01000000003",
                    Email = "giza@library.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Branches.AddRangeAsync(branches);
            await context.SaveChangesAsync();
        }
    }
}
