using LibrarymanagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Persistence.Configuration
{
    public class LoanBookConfiguration : IEntityTypeConfiguration<LoanBook>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<LoanBook> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }

}
