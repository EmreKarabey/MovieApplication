using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Rules;
using Application.Features.SavedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.SavedMovie.Queries.GetById
{
    public class GetByIdSavedMovieQuery : IRequest<GetByIdSavedMovieDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"GetByIdSavedMovieQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SavedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdSavedMovieHandler : IRequestHandler<GetByIdSavedMovieQuery, GetByIdSavedMovieDto>
    {
        private readonly SavedMovieBusinessRules _savedMovieBusinessRules;
        private readonly ISavedMovieRepository _savedMovieRepository;
        private readonly IMapper _mapper;

        public GetByIdSavedMovieHandler(SavedMovieBusinessRules savedMovieBusinessRules, ISavedMovieRepository savedMovieRepository, IMapper mapper)
        {
            _savedMovieBusinessRules = savedMovieBusinessRules;
            _savedMovieRepository = savedMovieRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdSavedMovieDto> Handle(GetByIdSavedMovieQuery request, CancellationToken cancellationToken)
        {
            await _savedMovieBusinessRules.NoSavedMovieFound(request.Id);
            var entity = await _savedMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdSavedMovieDto>(entity);

            return result;
        }
    }
}
