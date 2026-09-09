using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Comment.Queries.GetMovieId
{
    public class GetByMovieCommentIdQuery : IRequest<GetListResponse<GetByMovieCommentIdDto>>, ICachableRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }
        public PageRequest pageRequest { get; set; }

        public string? CacheKey => $"GetByMovieCommentIdQuery(Id={MovieID}{pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "Comments";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByMovieCommentIdHandler : IRequestHandler<GetByMovieCommentIdQuery, GetListResponse<GetByMovieCommentIdDto>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly CommentBusinessRules _commentBusinessRules;
        public GetByMovieCommentIdHandler(ICommentRepository commentRepository, CommentBusinessRules commentBusinessRules)
        {
            _commentRepository = commentRepository;
            _commentBusinessRules = commentBusinessRules;
        }

        public async Task<GetListResponse<GetByMovieCommentIdDto>> Handle(GetByMovieCommentIdQuery request, CancellationToken cancellationToken)
        {
            // await _commentBusinessRules.NoMovieFound(request.MovieID);

            Paginate<Domain.Entities.Comments> comments = await _commentRepository.GetListAsync(predicate: n => n.MovieID == request.MovieID, index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, include: n => n.Include(p => p.User).Include(p => p.SubComments), orderBy: n => n.OrderByDescending(p => p.CreatedAt));

            GetListResponse<GetByMovieCommentIdDto> getListResponse = new();

            foreach (var item in comments.Items)
            {
                var entity = new GetByMovieCommentIdDto();

                entity.EntityID = item.EntityID;
                entity.MovieID = item.MovieID;
                entity.UserName = item.User.FirstName + " " + item.User.LastName;
                entity.Content = item.Content;
                entity.CreatedAt = item.CreatedAt;
                entity.SubCommentsCount = item.SubComments?.Count ?? 0;

                getListResponse.Items.Add(entity);

            }
            if (comments.Count < comments.Size) getListResponse.PageSize = comments.Count;
            else getListResponse.PageSize = request.pageRequest.PageSize;

            getListResponse.PageIndex = comments.Index;
            return getListResponse;
        }
    }
}
