using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Application.Features.Login.Rules;
using Application.Features.MoviesCategory.Rules;
using Application.Services;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using FirebaseAdmin.Messaging;
using MediatR;

namespace Application.Features.MoviesCategory.Commands.Create
{
    public class CreatedMoviesCategoryCommand : IRequest<CreatedMoviesCategoryResponse>, ICacheRemoveRequest, ITransactionalRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }
        public Guid CategoryID { get; set; }

        public string? CacheKey => $"CreatedMoviesCategoryCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "MoviesCategories";
    }
    public class CreatedMoviesCategoryHandler : IRequestHandler<CreatedMoviesCategoryCommand, CreatedMoviesCategoryResponse>
    {
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        private readonly MovieCategoryBusinessRules _movieCategoryBusinessRules;
        private readonly IMapper _mapper;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationHubService _notificationHubService;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly IUserFCMTokenRepository _userFCMTokenRepository;

        public CreatedMoviesCategoryHandler(IMovieCategoryRepository movieCategoryRepository, MovieCategoryBusinessRules movieCategoryBusinessRules, IMapper mapper, ISubscriptionRepository subscriptionRepository, INotificationRepository notificationRepository, INotificationHubService notificationHubService, IMovieRepository movieRepository, IUserRepository userRepository, LoginBusinessRules loginBusinessRules, IUserFCMTokenRepository userFCMTokenRepository)
        {
            _movieCategoryRepository = movieCategoryRepository;
            _movieCategoryBusinessRules = movieCategoryBusinessRules;
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
            _notificationRepository = notificationRepository;
            _notificationHubService = notificationHubService;
            _movieRepository = movieRepository;
            _userRepository = userRepository;
            _loginBusinessRules = loginBusinessRules;
            _userFCMTokenRepository = userFCMTokenRepository;
        }

        public async Task<CreatedMoviesCategoryResponse> Handle(CreatedMoviesCategoryCommand request, CancellationToken cancellationToken)
        {
            await _movieCategoryBusinessRules.MoviesCategoryCannotBeDublicatedWhenInserted(request.MovieID, request.CategoryID);

            var entity = _mapper.Map<Domain.Entities.MoviesCategory>(request);

            var addMCategory = await _movieCategoryRepository.AddAsync(entity);

            var movie = await _movieRepository.GetAsync(predicate: m => m.EntityID == request.MovieID);

            var SubscriptiionUser = await _subscriptionRepository.ListAsync(movie.PublisherId);

            List<Domain.Entities.Notification> notifications = new List<Domain.Entities.Notification>();

            var publisher = await _userRepository.GetAsync(predicate: n => n.EntityID == movie.PublisherId);

            await _loginBusinessRules.NoUserFound(publisher.EntityID);

            foreach (var item in SubscriptiionUser)
            {
                var Notification = new Domain.Entities.Notification
                {
                    CreatedAt = DateTime.UtcNow,
                    Message = $"{publisher.FirstName + " " + publisher.LastName} yayıncı yeni bir film ekledi.",
                    UserID = item.UserId,
                    IsRead = false,
                    Url = $"https://localhost:7223/Movie/Play?videoUrl={movie.VideoURL}&MovieID={movie.EntityID}&movieName={movie.Name}"
                };

                notifications.Add(Notification);
            }

            await _notificationRepository.AddRangeAsync(notifications);



            foreach (var notification in notifications)
            {
                await _notificationHubService.SendNotificationToUser(notification.UserID);
                await _notificationHubService.NotificationCount(notification.UserID);
            }


            List<string> fcmTokens = await _userFCMTokenRepository.Tokens(notifications.Select(s => s.UserID).ToList());


            if (fcmTokens.Any())
            {
                foreach (var token in fcmTokens)
                {
                    var message = new FirebaseAdmin.Messaging.Message()
                    {
                        Token = token,
                        Notification = new FirebaseAdmin.Messaging.Notification()
                        {
                            Title = "Yeni Film Geldi!",
                            Body = $"{publisher.FirstName + " " + publisher.LastName} yayıncısı yeni bir film ekledi."
                        },
                        Webpush = new FirebaseAdmin.Messaging.WebpushConfig()
                        {
                            Headers = new Dictionary<string, string>() { { "Urgency", "high" } }
                        },
                        Data = new Dictionary<string, string>()
                        {
                            { "url",$"https://localhost:7223/Movie/Play?videoUrl={movie.VideoURL}&MovieID={movie.EntityID}&movieName={movie.Name}" }
                        }
                    };

                    try
                    {
                        await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"FireBase Hata:{ex.Message}");
                    }
                }
            }

            var result = _mapper.Map<CreatedMoviesCategoryResponse>(addMCategory);
            return result;
        }
    }
}

