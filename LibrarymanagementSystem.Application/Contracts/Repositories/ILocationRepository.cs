using LibrarymanagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Contracts.Repositories
{
    public interface ILocationRepository:IGenericRepository<Location>
    {
        public Task<bool> IsDuplicateLocation(string shelf, string floor, string section);
        public Task<bool> IsDuplicateLocationExclusive(int locationId,string shelf, string floor, string section);
    }
}
