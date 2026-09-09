using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Queries.GetById;
using Application.Features.Comment.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreApplication.Request;
using CoreApplication.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Comment.Queries.GetList
{
    public class GetListCommentQuery : IRequest<GetListResponse<GetListCommentDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListCommentQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Comments";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListCommentHandler : IRequestHandler<GetListCommentQuery, GetListResponse<GetListCommentDto>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        public GetListCommentHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListCommentDto>> Handle(GetListCommentQuery request, CancellationToken cancellationToken)
        {
            var list = await _commentRepository.GetListAsync(index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, predicate: null, include: n => n.Include(p => p.User));

            var result = new GetListResponse<GetListCommentDto>();
            foreach (var item in list.Items)
            {
                var entity = new GetListCommentDto();
                entity.UserName = item.User.FirstName + " " + item.User.LastName;
                entity.Content = item.Content;
                entity.MovieID = item.MovieID;
                entity.EntityID = item.EntityID;

                result.Items.Add(entity);
            }
            return result;
        }
    }
}
