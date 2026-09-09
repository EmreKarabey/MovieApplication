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
    public class NotificationSettingsConfiguration : IEntityTypeConfiguration<NotificationSettings>
    {
        public void Configure(EntityTypeBuilder<NotificationSettings> builder)
        {
            builder.ToTable("NotificationSettings").HasKey(n => n.EntityID);

            builder.Property(n => n.EntityID).HasColumnName("ID").IsRequired();
            builder.Property(n => n.UserId).HasColumnName("UserId").IsRequired();
            builder.Property(n => n.ChannelId).HasColumnName("ChannelId").IsRequired();
            builder.Property(n => n.IsNotificationEnabled).HasColumnName("IsNotificationEnabled").IsRequired();

            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasOne(n => n.User).WithMany().HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.Channel).WithMany().HasForeignKey(n => n.ChannelId).OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
