using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Category.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.Category.Queries.GetById
{
    public class GetByIdCategoryQuery : IRequest<GetByIdCategoryDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"GetByIdCategoryQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Categories";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdCategoryHandler : IRequestHandler<GetByIdCategoryQuery, GetByIdCategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly CategoryBusinessRules _categoryBusinessRules;

        public GetByIdCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper, CategoryBusinessRules categoryBusinessRules)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryBusinessRules = categoryBusinessRules;
        }

        public async Task<GetByIdCategoryDto> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
        {
            await _categoryBusinessRules.NoCategoryFound(request.Id);

            Domain.Entities.Category category = await _categoryRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdCategoryDto>(category);

            return result;
        }
    }
}
