using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Models;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Repositories
{
    public class FinePaymentRepository : GenericRepository<FinePayment>, IFinePaymentRepository
    {
        public FinePaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
