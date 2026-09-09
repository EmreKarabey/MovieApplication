using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.MoviesCategory.Queries.GetList
{
    public class GetListMovieCategoryQuery : IRequest<GetListResponse<GetListMovieCategoryDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListMovieCategoryQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "MoviesCategories";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListMovieCategoryHandler : IRequestHandler<GetListMovieCategoryQuery, GetListResponse<GetListMovieCategoryDto>>
    {
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        private readonly IMapper _mapper;

        public GetListMovieCategoryHandler(IMovieCategoryRepository movieCategoryRepository, IMapper mapper)
        {
            _movieCategoryRepository = movieCategoryRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListMovieCategoryDto>> Handle(GetListMovieCategoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _movieCategoryRepository.GetListAsync(predicate: null, index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, cancellationToken: cancellationToken);

            var result = _mapper.Map<GetListResponse<GetListMovieCategoryDto>>(entity);

            return result;
        }
    }
}
