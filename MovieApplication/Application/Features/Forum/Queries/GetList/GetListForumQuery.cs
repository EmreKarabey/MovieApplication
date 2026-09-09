using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Forum.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Forum.Queries.GetList
{
    public class GetListForumQuery : IRequest<GetListResponse<GetListForumDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListForumQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Forums";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListForumHandler : IRequestHandler<GetListForumQuery, GetListResponse<GetListForumDto>>
    {
        private readonly IForumRepository _forumRepository;
        private readonly IMapper _mapper;
        public GetListForumHandler(IForumRepository forumRepository, IMapper mapper)
        {
            _forumRepository = forumRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListForumDto>> Handle(GetListForumQuery request, CancellationToken cancellationToken)
        {
            var list = await _forumRepository.GetListAsync(index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, predicate: null, include: n => n.Include(p => p.User).Include(p => p.ForumCategory));

            var result = new GetListResponse<GetListForumDto>();
            foreach (var item in list.Items)
            {
                var entity = new GetListForumDto();
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
