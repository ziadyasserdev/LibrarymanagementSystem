using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IFinePaymentRepository FinePayments { get; }
        IReviewRepository Reviews { get; }
        IReviewReportRepository ReviewReports { get; }
        IBranchRepository Branches { get; }
        IFineRepository Fines { get; }
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        ILocationRepository Locations { get; }
        IPublisherRepository Publishers { get; }
        ICategoryRepository Categories { get; }
        IReservationRepository Reservations { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        //ILoanBookRepository LoanBookRepository { get; }
        ILoanBookRepository LoanBooks { get; }
      IBookCopyRepository BookCopies { get; }
        Task<int> SaveAsync();

    }
}
