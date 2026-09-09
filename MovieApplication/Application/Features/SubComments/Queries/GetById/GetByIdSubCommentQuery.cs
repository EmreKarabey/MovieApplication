using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Application.Features.SubComments.Queries.GetById
{
    public class GetByIdSubCommentQuery : IRequest<GetByIdSubCommentDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"GetByIdSubCommentQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SubComment";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdSubCommentHandler : IRequestHandler<GetByIdSubCommentQuery, GetByIdSubCommentDto>
    {
        private readonly ISubCommentRepository _subCommentRepository;
        private readonly IMapper _mapper;
        private readonly SubCommentBusinessRules _commentBusinessRules;
        public GetByIdSubCommentHandler(ISubCommentRepository subCommentRepository, IMapper mapper, SubCommentBusinessRules commentBusinessRules)
        {
            _subCommentRepository = subCommentRepository;
            _mapper = mapper;
            _commentBusinessRules = commentBusinessRules;
        }

        public async Task<GetByIdSubCommentDto> Handle(GetByIdSubCommentQuery request, CancellationToken cancellationToken)
        {
            await _commentBusinessRules.NoSubCommentFound(request.Id);

            SubComment comments = await _subCommentRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdSubCommentDto>(comments);

            return result;
        }
    }
}




