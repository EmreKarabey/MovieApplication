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

namespace Application.Features.UnlikedMovie.Commands.UnlikedMovie
{
    public class UnlikeMovieCommand : IRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid MovieId { get; set; }
        public int UserId { get; set; }

        public string? CacheKey => $"UnlikeMovieCommand Movie_Id:{MovieId}, User_Id:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "UnlikedMovies";
    }

    public class UnlikeMovieHandler : IRequestHandler<UnlikeMovieCommand>
    {
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;

        public UnlikeMovieHandler(IUnlikedMovieRepository unlikedMovieRepository)
        {
            _unlikedMovieRepository = unlikedMovieRepository;
        }

        public async Task Handle(UnlikeMovieCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.UnlikedMovie? unlikedMovie = await _unlikedMovieRepository.GetAsync(predicate: n => n.MovieID == request.MovieId && n.UserID == request.UserId, cancellationToken: cancellationToken);

            await _unlikedMovieRepository.DeleteAsync(unlikedMovie, permanent: true);
        }
    }
}
