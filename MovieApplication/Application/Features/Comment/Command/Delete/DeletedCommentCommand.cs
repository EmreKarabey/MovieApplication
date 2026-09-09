using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

namespace Application.Features.Comment.Command.Delete
{
    public class DeletedCommentCommand : IRequest<DeletedCommentResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"DeletedCommentCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Comments";
    }

    public class DeletedCommentHandler : IRequestHandler<DeletedCommentCommand, DeletedCommentResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly CommentBusinessRules _commentBusinessRules;
        public DeletedCommentHandler(ICommentRepository commentRepository, IMapper mapper, CommentBusinessRules commentBusinessRules)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _commentBusinessRules = commentBusinessRules;
        }

        public async Task<DeletedCommentResponse> Handle(DeletedCommentCommand request, CancellationToken cancellationToken)
        {
            await _commentBusinessRules.NoCommentFound(request.Id);

            Comments comments = await _commentRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteComment = await _commentRepository.DeleteAsync(comments);

            var result = _mapper.Map<DeletedCommentResponse>(deleteComment);

            return result;
        }
    }
}
