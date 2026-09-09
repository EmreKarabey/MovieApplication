using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreSecurity.Entities;
using CoreSecurity.Hashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations.UserConfig
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").HasKey(n => n.EntityID);

            builder.Property(n => n.FirstName).HasColumnName("FirstName").IsRequired();
            builder.Property(n => n.LastName).HasColumnName("LastName").IsRequired();
            builder.Property(n => n.EMail).HasColumnName("EMail").IsRequired();
            builder.Property(n => n.PasswordSalt).HasColumnName("PasswordSalt").IsRequired();
            builder.Property(n => n.PasswordHash).HasColumnName("PasswordHash").IsRequired();
            builder.Property(n => n.AccountType).HasColumnName("AccountType").IsRequired();
            builder.Property(n => n.Status).HasColumnName("Status").IsRequired();
            builder.Property(n => n.Code).HasColumnName("Code");
            builder.Property(n => n.CodeDuration).HasColumnName("CodeDuration");
            builder.Property(n => n.TwoFactor).HasColumnName("TwoFactor").IsRequired();
            builder.Property(n => n.DarkMode).HasColumnName("DarkMode").IsRequired();

            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasMany(n => n.OTPAuthenticators);
            builder.HasMany(n => n.EMailAuthenticators);
            builder.HasMany(n => n.UserOperationClaims);
            builder.HasMany(n => n.RefreshTokens);

            builder.HasIndex(n => n.EMail).IsUnique();

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);

            builder.HasData(getSeeds());

        }

        private IEnumerable<User> getSeeds()
        {
            List<User> users = new();
            HashingKeyHelper.CreatePasswordHash(Password: "123456Aa*",
                PasswordHash: out byte[] passwordHash,
                PasswordSalt: out byte[] passwordSalt);

            User admin = new()
            {
                EntityID = 1,
                FirstName = "Emre",
                LastName = "Admin",
                EMail = "admin@gmail.com",
                AccountType = "admin",
                Status = true,
                PasswordHash = Convert.FromBase64String("ipeXB3Wkjlg7/92Pq6ThoR62c+fOsW8eqg21DdRWeoyK+LD6KhMkN50a2V4lCuLrfD+sQA8tQLfJTunOUJ3mWQ=="),
                PasswordSalt = Convert.FromBase64String("rAZ1I2FciWBUnFckTGSf109P1SJBAKatsnhxsLsMD67WGzmHs6FndCXtyulovdSAooOUfkWkli531TudPM7V/OG3lFthZDGeDN6tXzHPN3GZBf8M3x5Bq985ySiBI33BRQL8XsxB7xotLxefnLETvfeJ+uVAppmvlVWF9kvU6/o="),
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),

            };

            users.Add(admin);

            return users;
        }
    }
}
