using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Http;

namespace Application.Features.Comment.Command.Update
{
    public class UpdateCommentCommand : IRequest<UpdateCommentResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid MovieID { get; set; }

        public string? CacheKey => $"UpdatedCommentCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Comments";
    }
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, UpdateCommentResponse>
    {
        private readonly CommentBusinessRules _commentBusinessRules;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public UpdateCommentHandler(ICommentRepository commentRepository, IMapper mapper, CommentBusinessRules commentBusinessRules)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _commentBusinessRules = commentBusinessRules;
        }

        public async Task<UpdateCommentResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {

            await _commentBusinessRules.NoCommentFound(request.Id);

            await _commentBusinessRules.UserAuthentication(request.Id);

            Comments comments = await _commentRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            comments = _mapper.Map(request, comments);

            var updateCommand = await _commentRepository.UpdateAsync(comments);

            var result = _mapper.Map<UpdateCommentResponse>(updateCommand);

            return result;
        }
    }
}
