using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Rules;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.FavoriteMovie.Queries.GetById
{
    public class GetByIdFavoriteMovieQuery : IRequest<GetByIdFavoriteMovieDto>, ICachableRequest, ILoggableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"GetByIdFavoriteMovieQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "FavoriteMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdFavoriteMovieHandler : IRequestHandler<GetByIdFavoriteMovieQuery, GetByIdFavoriteMovieDto>
    {
        private readonly FavoriteMovieBusinessRules _favoriteMovieBusinessRules;
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;
        private readonly IMapper _mapper;

        public GetByIdFavoriteMovieHandler(FavoriteMovieBusinessRules favoriteMovieBusinessRules, IFavoriteMovieRepository favoriteMovieRepository, IMapper mapper)
        {
            _favoriteMovieBusinessRules = favoriteMovieBusinessRules;
            _favoriteMovieRepository = favoriteMovieRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdFavoriteMovieDto> Handle(GetByIdFavoriteMovieQuery request, CancellationToken cancellationToken)
        {
            await _favoriteMovieBusinessRules.NoFavoriteMovieFound(request.Id);
            var entity = await _favoriteMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdFavoriteMovieDto>(entity);

            return result;
        }
    }
}
