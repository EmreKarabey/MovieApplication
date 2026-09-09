using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Rules;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.FavoriteMovie.Command.Update
{
    public class UpdateFavoriteMovieCommand : IRequest<UpdateFavoriteMovieResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public Guid MovieID { get; set; }

        public string? CacheKey => $"UpdateFavoriteMovieCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "FavoriteMovies";
    }

    public class UpdateFavoriteMovieHandler : IRequestHandler<UpdateFavoriteMovieCommand, UpdateFavoriteMovieResponse>
    {
        private readonly FavoriteMovieBusinessRules _favoriteMovieBusinessRules;
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;
        private readonly IMapper _mapper;

        public UpdateFavoriteMovieHandler(FavoriteMovieBusinessRules favoriteMovieBusinessRules, IFavoriteMovieRepository favoriteMovieRepository, IMapper mapper)
        {
            _favoriteMovieBusinessRules = favoriteMovieBusinessRules;
            _favoriteMovieRepository = favoriteMovieRepository;
            _mapper = mapper;
        }

        public async Task<UpdateFavoriteMovieResponse> Handle(UpdateFavoriteMovieCommand request, CancellationToken cancellationToken)
        {

            await _favoriteMovieBusinessRules.NoFavoriteMovieFound(request.Id);

            await _favoriteMovieBusinessRules.UserAuthentication(request.Id);

            Domain.Entities.FavoriteMovie favoriteMovie = await _favoriteMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            favoriteMovie = _mapper.Map(request, favoriteMovie);

            var updateFavoriteMovie = await _favoriteMovieRepository.UpdateAsync(favoriteMovie);

            var result = _mapper.Map<UpdateFavoriteMovieResponse>(updateFavoriteMovie);

            return result;
        }
    }
}
