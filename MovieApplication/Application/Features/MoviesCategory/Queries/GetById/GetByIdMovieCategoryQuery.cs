using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.MoviesCategory.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using MediatR;

namespace Application.Features.MoviesCategory.Queries.GetById
{
    public class GetByIdMovieCategoryQuery : IRequest<GetByIdMovieCategoryDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"GetByIdMovieCategoryQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "MoviesCategories";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdMovieCategoryHandler : IRequestHandler<GetByIdMovieCategoryQuery, GetByIdMovieCategoryDto>
    {
        private readonly MovieCategoryBusinessRules _movieCategoryBusinessRules;
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        private readonly IMapper _mapper;

        public GetByIdMovieCategoryHandler(IMovieCategoryRepository movieCategoryRepository, IMapper mapper, MovieCategoryBusinessRules movieCategoryBusinessRules)
        {
            _movieCategoryRepository = movieCategoryRepository;
            _mapper = mapper;
            _movieCategoryBusinessRules = movieCategoryBusinessRules;
        }

        public async Task<GetByIdMovieCategoryDto> Handle(GetByIdMovieCategoryQuery request, CancellationToken cancellationToken)
        {
            await _movieCategoryBusinessRules.NoMovieCategoryFound(request.Id);
            var entity = await _movieCategoryRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdMovieCategoryDto>(entity);

            return result;
        }
    }
}
