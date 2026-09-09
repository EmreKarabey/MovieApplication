using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Queries.GetById;
using Application.Features.LikedMovie.Rules;
using Application.Features.MoviesCategory.Queries.GetById;
using Application.Features.MoviesCategory.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using MediatR;

namespace Application.Features.LikedMovie.Queries.GetById
{
    public class GetByIdLikedMovieQuery : IRequest<GetByIdLikedMovieDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"GetByIdLikedMovieQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "LikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdLikedMovieHandler : IRequestHandler<GetByIdLikedMovieQuery, GetByIdLikedMovieDto>
    {
        private readonly LikedMovieBusinessRules _likedMovieBusinessRules;
        private readonly ILikedMovieRepository _likedMovieRepository;
        private readonly IMapper _mapper;

        public GetByIdLikedMovieHandler(LikedMovieBusinessRules likedMovieBusinessRules, ILikedMovieRepository likedMovieRepository, IMapper mapper)
        {
            _likedMovieBusinessRules = likedMovieBusinessRules;
            _likedMovieRepository = likedMovieRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdLikedMovieDto> Handle(GetByIdLikedMovieQuery request, CancellationToken cancellationToken)
        {
            await _likedMovieBusinessRules.NoLikedMovieFound(request.Id);
            var entity = await _likedMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdLikedMovieDto>(entity);

            return result;
        }
    }
}
