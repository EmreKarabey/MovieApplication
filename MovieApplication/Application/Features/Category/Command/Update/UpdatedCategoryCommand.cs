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

namespace Application.Features.Category.Command.Update
{
    public class UpdatedCategoryCommand : IRequest<UpdatedCategoryResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }

        public string? CacheKey => $"UpdatedCategoryCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Categories";
    }

    public class UpdatedCategoryHandler : IRequestHandler<UpdatedCategoryCommand, UpdatedCategoryResponse>
    {
        private readonly CategoryBusinessRules _categoryBusinessRules;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdatedCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper, CategoryBusinessRules categoryBusinessRules)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryBusinessRules = categoryBusinessRules;
        }

        public async Task<UpdatedCategoryResponse> Handle(UpdatedCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryBusinessRules.NoCategoryFound(request.Id);

            await _categoryBusinessRules.CategoryNameCannotBeDublicatedWhenInserted(request.CategoryName);

            Domain.Entities.Category? category = await _categoryRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            category = _mapper.Map(request, category);

            var updatedEntity = await _categoryRepository.UpdateAsync(category);

            var result = _mapper.Map<UpdatedCategoryResponse>(updatedEntity);

            return result;
        }
    }
}
