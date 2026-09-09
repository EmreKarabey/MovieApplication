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

namespace Application.Features.Comment.Command.Create
{
    public class CreatedCommentCommand : IRequest<CreatedCommentResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Content { get; set; }
        public Guid MovieID { get; set; }

        public string? CacheKey => $"CreatedCommentCommand Content:{Content}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Comments";
    }

    public class CreatedCommandHandler : IRequestHandler<CreatedCommentCommand, CreatedCommentResponse>
    {
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatedCommandHandler(ICommentRepository commentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<CreatedCommentResponse> Handle(CreatedCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User
     .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = _mapper.Map<Comments>(request);

            entity.UserID = currentUserId;

            var addComment = await _commentRepository.AddAsync(entity);

            var result = _mapper.Map<CreatedCommentResponse>(addComment);

            return result;
        }
    }
}
