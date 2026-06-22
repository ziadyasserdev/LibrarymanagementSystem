using LibrarymanagementSystem.Data.Models;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Persistence.SeedData
{
    public static class LocationSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Branches.Any())
                return; // لازم يبقى فيه فروع الأول

            if (!context.Locations.Any())
            {
                var firstBranch = context.Branches.First();
                var secondBranch = context.Branches.Skip(1).FirstOrDefault() ?? firstBranch;

                context.Locations.AddRange(
                    new Location
                    {
                        Shelf = "A1",
                        Floor = "1st Floor",
                        Section = "Fiction",
                        BranchId = firstBranch.Id
                    },
                    new Location
                    {
                        Shelf = "B2",
                        Floor = "1st Floor",
                        Section = "Science",
                        BranchId = firstBranch.Id
                    },
                    new Location
                    {
                        Shelf = "C3",
                        Floor = "2nd Floor",
                        Section = "History",
                        BranchId = secondBranch.Id
                    },
                    new Location
                    {
                        Shelf = "D4",
                        Floor = "2nd Floor",
                        Section = "Technology",
                        BranchId = secondBranch.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
