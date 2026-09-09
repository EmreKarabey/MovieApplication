using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Rules;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreSecurity.Entities;
using Domain.Entities;
using MediatR;

namespace Application.Features.LikedMovie.Command.UnlikeMovie
{
    public class LUnlikeMovieCommand : IRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid MovieId { get; set; }
        public int UserId { get; set; }

        public string? CacheKey => $"GetMovieIsLikedQuery Movie_Id:{MovieId}, User_Id:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "LikedMovies";
    }

    public class UnlikeMovieHandler : IRequestHandler<LUnlikeMovieCommand>
    {
        private readonly ILikedMovieRepository _likedMovieRepository;

        public UnlikeMovieHandler(ILikedMovieRepository likedMovieRepository)
        {
            _likedMovieRepository = likedMovieRepository;
        }

        public async Task Handle(LUnlikeMovieCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.LikedMovie? likedMovie = await _likedMovieRepository.GetAsync(predicate: n => n.MovieID == request.MovieId && n.UserID == request.UserId, cancellationToken: cancellationToken);

            await _likedMovieRepository.DeleteAsync(likedMovie, permanent: true);
        }
    }
}
