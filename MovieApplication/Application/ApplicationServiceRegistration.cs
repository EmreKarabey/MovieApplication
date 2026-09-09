using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Activities.Rules;
using Application.Features.Announcement.Rules;
using Application.Features.Category.Rules;
using Application.Features.Comment.Rules;
using Application.Features.FavoriteMovie.Rules;
using Application.Features.Forum.Rules;
using Application.Features.ForumCategory.Rules;
using Application.Features.History.Rules;
using Application.Features.LikedMovie.Rules;
using Application.Features.Login.Rules;
using Application.Features.Movies.Rules;
using Application.Features.MoviesCategory.Rules;
using Application.Features.Notification.Rules;
using Application.Features.NotificationSettings.Rules;
using Application.Features.SavedMovie.Rules;
using Application.Features.SubComments.Rules;
using Application.Features.Subscription.Rules;
using Application.Features.UnlikedMovie.Rules;
using CoreApplication.Pipelines.Authorization;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreApplication.Pipelines.Validation;
using CoreCrossingCuttingConcerns.SeriLog;
using CoreCrossingCuttingConcerns.SeriLog.Logger;
using CoreCrossingCuttingConcerns.SeriLog.Logger.FileLog;
using CoreCrossingCuttingConcerns.SeriLog.Logger.MSSql;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServiceRegistration(this IServiceCollection services)
        {

            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
                configuration.AddOpenBehavior(typeof(CacheRemoverRequest<,>));
                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
                configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
                configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            });

            services.AddSingleton<LoggerServiceBase, MSSqlLogger>();
            services.AddScoped<MoviesBusinessRules>();
            services.AddScoped<CategoryBusinessRules>();
            services.AddScoped<CommentBusinessRules>();
            services.AddScoped<MovieCategoryBusinessRules>();
            services.AddScoped<LoginBusinessRules>();
            services.AddScoped<LikedMovieBusinessRules>();
            services.AddScoped<SavedMovieBusinessRules>();
            services.AddScoped<AnnouncementBusinessRules>();
            services.AddScoped<UnlikedBusinessRules>();
            services.AddScoped<FavoriteMovieBusinessRules>();
            services.AddScoped<HistoryBusinessRules>();
            services.AddScoped<ActivitiesBusinessRules>();
            services.AddScoped<SubscriptionBusinessRules>();
            services.AddScoped<ForumBusinessRules>();
            services.AddScoped<ForumCategoryBusinessRules>();
            services.AddScoped<NotificationBusinessRules>();
            services.AddScoped<NotificationSettingsBusinessRules>();
            services.AddScoped<SubCommentBusinessRules>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
