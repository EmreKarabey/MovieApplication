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
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.ToTable("Announcements").HasKey(n => n.EntityID);

            builder.Property(n => n.EntityID).HasColumnName("ID").IsRequired();
            builder.Property(n => n.Title).HasColumnName("Title").IsRequired();
            builder.Property(n => n.Description).HasColumnName("Description").IsRequired();
            builder.Property(n => n.Subtitle1).HasColumnName("Subtitle1").IsRequired();
            builder.Property(n => n.Subtitle2).HasColumnName("Subtitle2").IsRequired();

            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
