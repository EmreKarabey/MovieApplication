using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Http;

namespace Application.Features.SubComments.Command.Update
{
    public class UpdateSubCommentCommand : IRequest<UpdateSubCommentResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid CommentID { get; set; }

        public string? CacheKey => $"UpdatedSubCommentCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SubComment";
    }
    public class UpdateSubCommentHandler : IRequestHandler<UpdateSubCommentCommand, UpdateSubCommentResponse>
    {
        private readonly SubCommentBusinessRules _commentBusinessRules;
        private readonly ISubCommentRepository _subCommentRepository;
        private readonly IMapper _mapper;

        public UpdateSubCommentHandler(ISubCommentRepository subCommentRepository, IMapper mapper, SubCommentBusinessRules commentBusinessRules)
        {
            _subCommentRepository = subCommentRepository;
            _mapper = mapper;
            _commentBusinessRules = commentBusinessRules;
        }

        public async Task<UpdateSubCommentResponse> Handle(UpdateSubCommentCommand request, CancellationToken cancellationToken)
        {

            await _commentBusinessRules.NoSubCommentFound(request.Id);

            await _commentBusinessRules.UserAuthentication(request.Id);

            SubComment comments = await _subCommentRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            comments = _mapper.Map(request, comments);

            var updateCommand = await _subCommentRepository.UpdateAsync(comments);

            var result = _mapper.Map<UpdateSubCommentResponse>(updateCommand);

            return result;
        }
    }
}




