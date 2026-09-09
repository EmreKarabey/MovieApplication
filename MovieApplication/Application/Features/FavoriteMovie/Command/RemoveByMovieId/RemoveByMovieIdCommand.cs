using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Command.UnlikeMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.FavoriteMovie.Command.RemoveByMovieId
{
    public class RemoveByMovieIdCommand : IRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid MovieId { get; set; }
        public int UserId { get; set; }

        public string? CacheKey => $"RemoveByMovieIdQuery Movie_Id:{MovieId}, User_Id:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "FavoriteMovies";
    }

    public class RemoveByMovieIdHandler : IRequestHandler<RemoveByMovieIdCommand>
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;

        public RemoveByMovieIdHandler(IFavoriteMovieRepository favoriteMovieRepository)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
        }

        public async Task Handle(RemoveByMovieIdCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.FavoriteMovie? favoriteMovie = await _favoriteMovieRepository.GetAsync(predicate: n => n.MovieID == request.MovieId && n.UserID == request.UserId, cancellationToken: cancellationToken);

            await _favoriteMovieRepository.DeleteAsync(favoriteMovie, permanent: true);
        }
    }
}
