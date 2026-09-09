using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Movies.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreApplication.Request;
using CoreApplication.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Features.Movies.Queries.GetById
{
    public class GetByIdMovieQuery : IRequest<GetByIdMovieDto>, ICachableRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public string CacheKey => $"GetMovieListQuery Id:{Id}";

        public string? CacheGroupKey => "Movies";

        public bool ByPassCache { get; }
        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdMoviewListHandler : IRequestHandler<GetByIdMovieQuery, GetByIdMovieDto>
    {
        private readonly MoviesBusinessRules _moviesBusinessRules;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetByIdMoviewListHandler(IMovieRepository movieRepository, IMapper mapper, MoviesBusinessRules moviesBusinessRules)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _moviesBusinessRules = moviesBusinessRules;
        }

        public async Task<GetByIdMovieDto> Handle(GetByIdMovieQuery request, CancellationToken cancellationToken)
        {
            await _moviesBusinessRules.NoMovieFound(request.Id);

            Movie? movie = await _movieRepository.GetAsync(predicate: n => n.EntityID == request.Id, include: m => m.Include(x => x.Publisher), withDeleted: true, cancellationToken: cancellationToken, enableTracking: true);

            var result = _mapper.Map<GetByIdMovieDto>(movie);
            return result;
        }
    }
}
