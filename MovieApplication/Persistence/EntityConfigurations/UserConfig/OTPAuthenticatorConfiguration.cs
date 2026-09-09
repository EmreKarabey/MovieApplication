using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreSecurity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations.UserConfig
{
    public class OTPAuthenticatorConfiguration : IEntityTypeConfiguration<OTPAuthenticator>
    {
        public void Configure(EntityTypeBuilder<OTPAuthenticator> builder)
        {
            builder.ToTable("OTPAuthenticators").HasKey(n => n.EntityID);

            builder.Property(n => n.UserId).HasColumnName("UserId").IsRequired();
            builder.Property(n => n.SecretKey).HasColumnName("SecretKey").IsRequired();
            builder.Property(n => n.IsVerifed).HasColumnName("IsVerifed").IsRequired();

            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasOne(n => n.User);

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
