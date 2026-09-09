using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Application.Features.Category.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.Category.Command.Create
{
    public class CreatedCategoryCommand : IRequest<CreatedCategoryResponse>, ICacheRemoveRequest, ILoggableRequest, ITransactionalRequest
    {
        public string CategoryName { get; set; }

        public string? CacheKey => $"CreatedMovieCommand CategoryName:{CategoryName}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Categories";
    }

    public class CreateCategoryHandler : IRequestHandler<CreatedCategoryCommand, CreatedCategoryResponse>
    {
        private readonly CategoryBusinessRules _categoryBusinessRules;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper, CategoryBusinessRules categoryBusinessRules)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryBusinessRules = categoryBusinessRules;
        }

        public async Task<CreatedCategoryResponse> Handle(CreatedCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryBusinessRules.CategoryNameCannotBeDublicatedWhenInserted(request.CategoryName);

            var entity = _mapper.Map<Domain.Entities.Category>(request);

            var addCategory = await _categoryRepository.AddAsync(entity);

            var result = _mapper.Map<CreatedCategoryResponse>(addCategory);

            return result;
        }
    }
}
