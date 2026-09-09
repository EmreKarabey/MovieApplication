using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.SubComments.Queries.GetById;
using Application.Features.SubComments.Rules;
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

namespace Application.Features.SubComments.Queries.GetList
{
    public class GetListSubCommentQuery : IRequest<GetListResponse<GetListSubCommentDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListSubCommentQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SubComment";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListSubCommentHandler : IRequestHandler<GetListSubCommentQuery, GetListResponse<GetListSubCommentDto>>
    {
        private readonly ISubCommentRepository _subCommentRepository;
        private readonly IMapper _mapper;
        public GetListSubCommentHandler(ISubCommentRepository subCommentRepository, IMapper mapper)
        {
            _subCommentRepository = subCommentRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListSubCommentDto>> Handle(GetListSubCommentQuery request, CancellationToken cancellationToken)
        {
            var list = await _subCommentRepository.GetListAsync(index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, predicate: null, include: n => n.Include(p => p.User));

            var result = new GetListResponse<GetListSubCommentDto>();
            foreach (var item in list.Items)
            {
                var entity = new GetListSubCommentDto();
                entity.UserName = item.User.FirstName + " " + item.User.LastName;
                entity.Content = item.Content;
                entity.CommentID = item.CommentID;
                entity.EntityID = item.EntityID;

                result.Items.Add(entity);
            }
            return result;
        }
    }
}




