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
    public class FavoriteMoviesConfiguration : IEntityTypeConfiguration<FavoriteMovie>
    {
        public void Configure(EntityTypeBuilder<FavoriteMovie> builder)
        {
            builder.ToTable("FavoriteMovies").HasKey(n => n.EntityID);

            builder.Property(n => n.EntityID).HasColumnName("ID").IsRequired();
            builder.Property(n => n.MovieID).HasColumnName("MovieID").IsRequired();
            builder.Property(n => n.UserID).HasColumnName("UserID").IsRequired();

            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasOne(n => n.Movie);
            builder.HasOne(n => n.User);

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);

        }
    }
}
