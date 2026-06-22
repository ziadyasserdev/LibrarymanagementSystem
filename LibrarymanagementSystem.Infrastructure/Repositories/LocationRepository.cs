using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Models;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Repositories
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        private readonly ApplicationDbContext dbContext;
        public LocationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsDuplicateLocation(string shelf, string floor, string section)
        {
            return await dbContext.Locations.AnyAsync(c=>c.Shelf == shelf 
            && c.Floor == floor 
            && c.Section == section 
            && !c.IsDeleted);
        }

        public async Task<bool> IsDuplicateLocationExclusive(int locationId,string shelf, string floor, string section)
        {
            return await dbContext.Locations.AnyAsync(c => c.Shelf == shelf
            && c.Floor == floor
            && c.Section == section
            && !c.IsDeleted 
            && c.Id != locationId);
           
        }
    }
}
