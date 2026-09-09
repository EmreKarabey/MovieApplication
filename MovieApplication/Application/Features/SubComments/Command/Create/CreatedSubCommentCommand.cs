using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreSecurity.Entities;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.SubComments.Command.Create
{
    public class CreatedSubCommentCommand : IRequest<CreatedSubCommentResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Content { get; set; }
        public Guid CommentID { get; set; }

        public string? CacheKey => $"CreatedSubCommentCommand Content:{Content}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SubComment";
    }

    public class CreatedCommandHandler : IRequestHandler<CreatedSubCommentCommand, CreatedSubCommentResponse>
    {
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly ISubCommentRepository _subCommentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatedCommandHandler(ISubCommentRepository subCommentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules)
        {
            _subCommentRepository = subCommentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<CreatedSubCommentResponse> Handle(CreatedSubCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User
     .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = _mapper.Map<SubComment>(request);

            entity.UserID = currentUserId;

            var addSubComment = await _subCommentRepository.AddAsync(entity);

            var result = _mapper.Map<CreatedSubCommentResponse>(addSubComment);

            return result;
        }
    }
}




