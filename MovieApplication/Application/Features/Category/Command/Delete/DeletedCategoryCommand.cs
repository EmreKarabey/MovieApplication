using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Category.Command.Create;
using Application.Features.Category.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.Category.Command.Delete
{
    public class DeletedCategoryCommand : IRequest<DeletedCategoryResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"DeletedCategoryCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Categories";
    }

    public class DeletedCategoryHandler : IRequestHandler<DeletedCategoryCommand, DeletedCategoryResponse>
    {
        private readonly CategoryBusinessRules _categoryBusinessRules;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public DeletedCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper, CategoryBusinessRules categoryBusinessRules)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryBusinessRules = categoryBusinessRules;
        }

        public async Task<DeletedCategoryResponse> Handle(DeletedCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryBusinessRules.NoCategoryFound(request.Id);

            Domain.Entities.Category category = await _categoryRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteEntity = await _categoryRepository.DeleteAsync(category);

            var result = _mapper.Map<DeletedCategoryResponse>(deleteEntity);

            return result;
        }
    }
}
