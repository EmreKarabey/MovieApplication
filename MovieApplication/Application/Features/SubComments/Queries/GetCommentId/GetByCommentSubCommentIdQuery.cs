using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.SubComments.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SubComments.Queries.GetCommentId
{
    public class GetByCommentSubCommentIdQuery : IRequest<GetListResponse<GetByCommentSubCommentIdDto>>, ICachableRequest, ILoggableRequest
    {
        public Guid CommentID { get; set; }
        public PageRequest pageRequest { get; set; }

        public string? CacheKey => $"GetByCommentSubCommentIdQuery(Id={CommentID}{pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "SubComment";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByCommentSubCommentIdHandler : IRequestHandler<GetByCommentSubCommentIdQuery, GetListResponse<GetByCommentSubCommentIdDto>>
    {
        private readonly ISubCommentRepository _subCommentRepository;
        private readonly SubCommentBusinessRules _commentBusinessRules;
        public GetByCommentSubCommentIdHandler(ISubCommentRepository subCommentRepository, SubCommentBusinessRules commentBusinessRules)
        {
            _subCommentRepository = subCommentRepository;
            _commentBusinessRules = commentBusinessRules;
        }

        public async Task<GetListResponse<GetByCommentSubCommentIdDto>> Handle(GetByCommentSubCommentIdQuery request, CancellationToken cancellationToken)
        {

            Paginate<Domain.Entities.SubComment> comments = await _subCommentRepository.GetListAsync(predicate: n => n.CommentID == request.CommentID, index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, include: n => n.Include(p => p.User), orderBy: n => n.OrderByDescending(p => p.CreatedAt));

            GetListResponse<GetByCommentSubCommentIdDto> getListResponse = new();

            foreach (var item in comments.Items)
            {
                var entity = new GetByCommentSubCommentIdDto();

                entity.EntityID = item.EntityID;
                entity.CommentID = item.CommentID;
                entity.UserName = item.User.FirstName + " " + item.User.LastName;
                entity.Content = item.Content;
                entity.CreatedAt = item.CreatedAt;

                getListResponse.Items.Add(entity);

            }
            if (comments.Count < comments.Size) getListResponse.PageSize = comments.Count;
            else getListResponse.PageSize = request.pageRequest.PageSize;

            getListResponse.PageIndex = comments.Index;
            return getListResponse;
        }
    }
}




