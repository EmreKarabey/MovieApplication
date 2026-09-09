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
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies").HasKey(n => n.EntityID);

            builder.Property(n => n.EntityID).HasColumnName("ID").IsRequired();
            builder.Property(n => n.Name).HasColumnName("Name").IsRequired();
            builder.Property(n => n.ImageURL).HasColumnName("ImageURL").IsRequired();
            builder.Property(n => n.VideoURL).HasColumnName("VideoURL").IsRequired();
            builder.Property(n => n.Description).HasColumnName("Description").IsRequired();
            builder.Property(n => n.ProducerName).HasColumnName("ProducerName").IsRequired();
            builder.Property(n => n.ReleaseDate).HasColumnName("ReleaseDate").IsRequired();
            builder.Property(n => n.PublisherId).HasColumnName("PublisherId").IsRequired();


            builder.Property(n => n.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(n => n.DeletedAt).HasColumnName("DeletedAt");
            builder.Property(n => n.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasMany(n => n.SavedMovies);
            builder.HasMany(n => n.LikedMovies);
            builder.HasMany(n => n.MoviesCategories);

            builder.HasOne(n => n.Publisher).WithMany()
            .HasForeignKey(m => m.PublisherId)
            .OnDelete(DeleteBehavior.NoAction); ;

            builder.HasQueryFilter(n => !n.DeletedAt.HasValue);
        }
    }
}
