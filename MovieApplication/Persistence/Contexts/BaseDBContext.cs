using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CoreSecurity.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Persistence.Contexts
{
    public class BaseDBContext : DbContext
    {
        protected IConfiguration _configuration;

        public DbSet<Movie> Movies { get; set; }

        #region User
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaim { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<OTPAuthenticator> OTPAuthenticators { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EMailAuthenticator> EMailAuthenticators { get; set; }
        #endregion

        public DbSet<MoviesCategory> MoviesCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SavedMovie> SavedMovies { get; set; }
        public DbSet<LikedMovie> LikedMovies { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<UnlikedMovie> UnlikedMovies { get; set; }
        public DbSet<FavoriteMovie> FavoriteMovies { get; set; }
        public DbSet<Activities> Activities { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationSettings> NotificationSettings { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<UserFCMToken> UserFCMTokens { get; set; }

        public BaseDBContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }


    }
}
