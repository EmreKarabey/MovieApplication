using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.Comment.Queries.GetById
{
    public class GetByIdCommentQuery : IRequest<GetByIdCommentDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"GetByIdCommentQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Comments";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdCommentHandler : IRequestHandler<GetByIdCommentQuery, GetByIdCommentDto>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly CommentBusinessRules _commentBusinessRules;
        public GetByIdCommentHandler(ICommentRepository commentRepository, IMapper mapper, CommentBusinessRules commentBusinessRules)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _commentBusinessRules = commentBusinessRules;
        }

        public async Task<GetByIdCommentDto> Handle(GetByIdCommentQuery request, CancellationToken cancellationToken)
        {
            await _commentBusinessRules.NoCommentFound(request.Id);

            Comments comments = await _commentRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdCommentDto>(comments);

            return result;
        }
    }
}
