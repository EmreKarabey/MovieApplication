using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Category.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.Category.Queries.GetList
{
    public class GetListCategoryQuery : IRequest<GetListResponse<GetListCategoryDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListCategoryQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Categories";

        public TimeSpan? SlidingExpiration { get; }
    }
    public class GetListCategoryHandler : IRequestHandler<GetListCategoryQuery, GetListResponse<GetListCategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetListCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListCategoryDto>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
        {
            var list = await _categoryRepository.GetListAsync(predicate: null, index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize);

            var result = _mapper.Map<GetListResponse<GetListCategoryDto>>(list);

            return result;
        }
    }
}
