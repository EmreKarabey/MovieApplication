using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class MoviesCategoryConfiguration : IEntityTypeConfiguration<MoviesCategory>
    {
        public void Configure(EntityTypeBuilder<MoviesCategory> builder)
        {
            builder.ToTable("MoviesCategories").HasKey(n => n.EntityID);

            builder.Property(n => n.EntityID).HasColumnName("ID").IsRequired();
            builder.Property(n => n.MovieID).HasColumnName("MovieID").IsRequired();
            builder.Property(n => n.CategoryID).HasColumnName("CategoryID").IsRequired();

            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasOne(n => n.Movie).WithMany(m => m.MoviesCategories)
               .HasForeignKey(mc => mc.MovieID);

            builder.HasOne(n => n.Category).WithMany(c => c.MoviesCategories)
               .HasForeignKey(mc => mc.CategoryID);

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
