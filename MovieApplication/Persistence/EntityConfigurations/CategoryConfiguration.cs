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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories").HasKey(n => n.EntityID);

            builder.Property(n => n.EntityID).HasColumnName("ID").IsRequired();
            builder.Property(n => n.CategoryName).HasColumnName("CategoryName").IsRequired();

            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasMany(n => n.MoviesCategories);

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
