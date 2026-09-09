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
    public class UserFCMTokenConfiguration : IEntityTypeConfiguration<UserFCMToken>
    {
        public void Configure(EntityTypeBuilder<UserFCMToken> builder)
        {
            builder.ToTable("UserFCMTokens").HasKey(n => n.EntityID);

            builder.Property(n => n.EntityID).HasColumnName("ID").IsRequired();
            builder.Property(n => n.UserId).HasColumnName("UserId").IsRequired();
            builder.Property(n => n.Token).HasColumnName("Token").IsRequired();


            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasOne(n => n.User);


            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
