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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.CategoryId);
           builder.HasMany(c=>c.Books)
                .WithOne(c=> c.Category)
                .HasForeignKey(c=>c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
