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
    public class RefreshTokensConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens").HasKey(n => n.EntityID);

            builder.Property(n => n.UserId).HasColumnName("UserId").IsRequired();
            builder.Property(n => n.Token).HasColumnName("Token").IsRequired();
            builder.Property(n => n.Expires).HasColumnName("Expires").IsRequired();
            builder.Property(n => n.CreatedByIp).HasColumnName("CreatedByIp").IsRequired();
            builder.Property(n => n.Revoked).HasColumnName("Revoked");
            builder.Property(n => n.RevokedByIp).HasColumnName("RevokedByIp");
            builder.Property(n => n.ReplacedByToken).HasColumnName("ReplacedByToken");
            builder.Property(n => n.ReasonRevoked).HasColumnName("ReasonRevoked");

            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasOne(n => n.User);

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
