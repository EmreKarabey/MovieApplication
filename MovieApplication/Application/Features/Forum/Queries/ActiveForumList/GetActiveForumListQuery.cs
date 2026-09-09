using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Forum.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Forum.Queries.ActiveForumList
{
    public class GetActiveForumListQuery : IRequest<GetListResponse<GetActiveForumListDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetActiveForumListQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Forums";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetActiveForumListHandler : IRequestHandler<GetActiveForumListQuery, GetListResponse<GetActiveForumListDto>>
    {
        private readonly IForumRepository _forumRepository;
        private readonly IMapper _mapper;
        public GetActiveForumListHandler(IForumRepository forumRepository, IMapper mapper)
        {
            _forumRepository = forumRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetActiveForumListDto>> Handle(GetActiveForumListQuery request, CancellationToken cancellationToken)
        {
            var list = await _forumRepository.GetListAsync(index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, predicate: n => n.Status == "Onaylandı", include: n => n.Include(p => p.User).Include(p => p.ForumCategory), orderBy: n => n.OrderByDescending(p => p.CreatedAt));

            var result = new GetListResponse<GetActiveForumListDto>();
            foreach (var item in list.Items)
            {
                var entity = new GetActiveForumListDto();
                entity.UserName = item.User.FirstName + " " + item.User.LastName;
                entity.Title = item.Title;
                entity.Details = item.Details;
                entity.ForumCategoryId = item.ForumCategoryId;
                entity.EntityID = item.EntityID;
                entity.ForumCategoryName = item.ForumCategory.Name;
                entity.CreatedAt = item.CreatedAt;
                entity.UserId = item.UserId;

                result.Items.Add(entity);
            }
            return result;
        }
    }
}
