using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Application.Features.SubComments.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.SubComments.Command.Delete
{
    public class DeletedSubCommentCommand : IRequest<DeletedSubCommentResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"DeletedSubCommentCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SubComment";
    }

    public class DeletedSubCommentHandler : IRequestHandler<DeletedSubCommentCommand, DeletedSubCommentResponse>
    {
        private readonly ISubCommentRepository _subCommentRepository;
        private readonly IMapper _mapper;
        private readonly SubCommentBusinessRules _commentBusinessRules;
        public DeletedSubCommentHandler(ISubCommentRepository subCommentRepository, IMapper mapper, SubCommentBusinessRules commentBusinessRules)
        {
            _subCommentRepository = subCommentRepository;
            _mapper = mapper;
            _commentBusinessRules = commentBusinessRules;
        }

        public async Task<DeletedSubCommentResponse> Handle(DeletedSubCommentCommand request, CancellationToken cancellationToken)
        {
            await _commentBusinessRules.NoSubCommentFound(request.Id);

            SubComment comments = await _subCommentRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteSubComment = await _subCommentRepository.DeleteAsync(comments);

            var result = _mapper.Map<DeletedSubCommentResponse>(deleteSubComment);

            return result;
        }
    }
}




