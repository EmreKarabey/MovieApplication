using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Queries.GetById;
using Application.Features.LikedMovie.Rules;
using Application.Features.MoviesCategory.Queries.GetById;
using Application.Features.MoviesCategory.Rules;
using Application.Features.UnlikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using MediatR;

namespace Application.Features.UnlikedMovie.Queries.GetById
{
    public class GetByIdUnlikedMovieQuery : IRequest<GetByIdUnlikedMovieDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"GetByIdUnlikedMovieQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "UnlikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdLikedMovieHandler : IRequestHandler<GetByIdUnlikedMovieQuery, GetByIdUnlikedMovieDto>
    {
        private readonly UnlikedBusinessRules _unlikedMovieBusinessRules;
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;
        private readonly IMapper _mapper;

        public GetByIdLikedMovieHandler(UnlikedBusinessRules unlikedMovieBusinessRules, IUnlikedMovieRepository unlikedMovieRepository, IMapper mapper)
        {
            _unlikedMovieBusinessRules = unlikedMovieBusinessRules;
            _unlikedMovieRepository = unlikedMovieRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdUnlikedMovieDto> Handle(GetByIdUnlikedMovieQuery request, CancellationToken cancellationToken)
        {
            await _unlikedMovieBusinessRules.NoUnlikedMovieFound(request.Id);
            var entity = await _unlikedMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdUnlikedMovieDto>(entity);

            return result;
        }
    }
}
