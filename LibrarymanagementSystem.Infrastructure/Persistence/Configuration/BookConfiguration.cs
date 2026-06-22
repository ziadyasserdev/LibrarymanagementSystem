using LibrarymanagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Persistence.Configuration
{
  
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.Id);
            //builder.HasMany(x=>x.LoanBooks)
            //    .WithOne(x=>x.Book)
            //    .HasForeignKey(c=>c.BookId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
