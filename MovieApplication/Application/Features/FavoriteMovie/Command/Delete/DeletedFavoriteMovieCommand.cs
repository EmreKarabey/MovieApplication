using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Rules;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.FavoriteMovie.Command.Delete
{
    public class DeletedFavoriteMovieCommand : IRequest<DeletedFavoriteMovieResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"DeleteFavoriteMovieCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "FavoriteMovies";
    }

    public class DeleteFavoriteMovieHandler : IRequestHandler<DeletedFavoriteMovieCommand, DeletedFavoriteMovieResponse>
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;
        private readonly IMapper _mapper;
        private readonly FavoriteMovieBusinessRules _favoriteMovieBusinessRules;

        public DeleteFavoriteMovieHandler(IFavoriteMovieRepository favoriteMovieRepository, IMapper mapper, FavoriteMovieBusinessRules favoriteMovieBusinessRules)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
            _mapper = mapper;
            _favoriteMovieBusinessRules = favoriteMovieBusinessRules;
        }

        public async Task<DeletedFavoriteMovieResponse> Handle(DeletedFavoriteMovieCommand request, CancellationToken cancellationToken)
        {
            await _favoriteMovieBusinessRules.NoFavoriteMovieFound(request.Id);

            Domain.Entities.FavoriteMovie favoriteMovie = await _favoriteMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteFavoriteMovie = await _favoriteMovieRepository.DeleteAsync(favoriteMovie, permanent: false);

            var result = _mapper.Map<DeletedFavoriteMovieResponse>(deleteFavoriteMovie);

            return result;
        }
    }
}
