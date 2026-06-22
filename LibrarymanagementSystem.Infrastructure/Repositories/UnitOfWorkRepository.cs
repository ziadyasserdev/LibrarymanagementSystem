using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext dbContext;
        public IBookRepository Books { get; }

        public IAuthorRepository Authors { get; }

        public ICategoryRepository Categories { get; }

        public ILoanBookRepository LoanBooks{ get; }

        public ILocationRepository Locations { get; }

        public IPublisherRepository Publishers { get; }

     

        public IFinePaymentRepository FinePayments {  get; }

        public IFineRepository Fines {  get; }

        public IReviewRepository Reviews { get; }

        public IReviewReportRepository ReviewReports { get; }

        public IBranchRepository Branches {  get; }

        public IBookCopyRepository BookCopies { get; }

        public IReservationRepository Reservations { get; }

        public UnitOfWorkRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            Reservations = new ReservationRepository(dbContext);
            Books = new BookRepository(dbContext);
            Authors = new AuthorRepository(dbContext);
            Categories = new CategoryRepository(dbContext);
            LoanBooks = new LoanBookRepository(dbContext);
            Publishers = new PublisherRepository(dbContext);
            Locations = new LocationRepository(dbContext);
            Fines = new FineRepository(dbContext);
            FinePayments = new FinePaymentRepository(dbContext);
            Reviews = new ReviewRepository(dbContext);
            ReviewReports= new ReviewReportRepository(dbContext);
            Branches = new BranchRepository(dbContext);
            BookCopies = new BookCopyRepository(dbContext);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await dbContext.Database.BeginTransactionAsync();
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
