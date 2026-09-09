using Application.Features.LikedMovie.Queries.GetMovieLikedCount;
using Application.Features.LikedMovie.Queries.IsLiked;
using Application.Services.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace MovieApplication.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly ILikedMovieRepository _likedMovieRepository;
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;
        private readonly INotificationRepository _notificationRepository;

        public SignalRHub(ILikedMovieRepository likedMovieRepository, IUnlikedMovieRepository unlikedMovieRepository, INotificationRepository notificationRepository)
        {
            _likedMovieRepository = likedMovieRepository;
            _unlikedMovieRepository = unlikedMovieRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task JoinMovieGroup(string MovieId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, MovieId);
        }

        public async Task SendCount(Guid MovieId)
        {
            int count = await _likedMovieRepository.MovieLikedCount(predicate: n => n.MovieID == MovieId);
            await Clients.Group(MovieId.ToString()).SendAsync("ReceiveMovieLikedCount", count);


            int unlikedCount = await _unlikedMovieRepository.MovieUnlikedCount(predicate: n => n.MovieID == MovieId);
            await Clients.Group(MovieId.ToString()).SendAsync("ReceiveMovieUnlikedCount", unlikedCount);
        }

        public async Task JoinNotificationGroup(string UserId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, UserId);
        }

    }
}
