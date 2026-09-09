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
    public class EMailAuthenticatorConfiguration : IEntityTypeConfiguration<EMailAuthenticator>
    {
        public void Configure(EntityTypeBuilder<EMailAuthenticator> builder)
        {
            builder.ToTable("EMailAuthenticators").HasKey(n => n.EntityID);

            builder.Property(n => n.UserId).HasColumnName("UserId").IsRequired();
            builder.Property(n => n.ActivationKey).HasColumnName("ActivationKey");
            builder.Property(n => n.IsVerifed).HasColumnName("IsVerifed").IsRequired();

            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasOne(n => n.User);

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
