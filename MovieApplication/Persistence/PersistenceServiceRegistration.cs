using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BaseDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                )));

            services.AddScoped<DbContext>(provider => provider.GetRequiredService<BaseDBContext>());

            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IMovieCategoryRepository, MovieCategoryRepository>();
            services.AddScoped<ILikedMovieRepository, LikedMovieRepository>();
            services.AddScoped<ISavedMovieRepository, SavedMovieRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IUnlikedMovieRepository, UnlikedMovieRepository>();
            services.AddScoped<IFavoriteMovieRepository, FavoriteMovieRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IOTPAuthenticatorRepository, OTPAuthenticatorRepository>();
            services.AddScoped<IActivitiesRepository, ActivitiesRepository>();
            services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IForumRepository, ForumRepository>();
            services.AddScoped<IForumCategoryRepository, ForumCategoryRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationSettingsRepository, NotificationSettingsRepository>();
            services.AddScoped<IUserFCMTokenRepository, UserFCMTokenRepository>();
            services.AddScoped<ISubCommentRepository, SubCommentRepository>();

            return services;
        }
    }
}
