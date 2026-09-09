using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.ForumCategory.Queries.GetList
{
    public class GetListForumCategoryQuery : IRequest<GetListResponse<GetListForumCategoryDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListForumCategoryQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "ForumCategories";

        public TimeSpan? SlidingExpiration { get; }
    }
    public class GetListForumCategoryHandler : IRequestHandler<GetListForumCategoryQuery, GetListResponse<GetListForumCategoryDto>>
    {
        private readonly IForumCategoryRepository _forumCategoryRepository;
        private readonly IMapper _mapper;

        public GetListForumCategoryHandler(IForumCategoryRepository forumCategoryRepository, IMapper mapper)
        {
            _forumCategoryRepository = forumCategoryRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListForumCategoryDto>> Handle(GetListForumCategoryQuery request, CancellationToken cancellationToken)
        {
            var list = await _forumCategoryRepository.GetListAsync(predicate: null, index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize);

            var result = _mapper.Map<GetListResponse<GetListForumCategoryDto>>(list);

            return result;
        }
    }
}
