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
    public class ForumCategoryConfiguration : IEntityTypeConfiguration<ForumCategory>
    {
        public void Configure(EntityTypeBuilder<ForumCategory> builder)
        {
            builder.ToTable("ForumCategories").HasKey(n => n.EntityID);

            builder.Property(n => n.EntityID).HasColumnName("ID").IsRequired();
            builder.Property(n => n.Name).HasColumnName("Name").IsRequired();


            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasMany(n => n.Forums);

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
