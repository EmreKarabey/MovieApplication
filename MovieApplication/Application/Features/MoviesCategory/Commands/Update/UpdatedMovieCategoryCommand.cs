using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Category.Command.Update;
using Application.Features.Category.Rules;
using Application.Features.MoviesCategory.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using MediatR;

namespace Application.Features.MoviesCategory.Commands.Update
{
    public class UpdatedMovieCategoryCommand : IRequest<UpdatedMovieCategoryResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }
        public Guid MovieID { get; set; }
        public Guid CategoryID { get; set; }


        public string? CacheKey => $"UpdatedMovieCategoryCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "MoviesCategories";
    }

    public class UpdatedCategoryHandler : IRequestHandler<UpdatedMovieCategoryCommand, UpdatedMovieCategoryResponse>
    {
        private readonly MovieCategoryBusinessRules _movieCategoryBusinessRules;
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        private readonly IMapper _mapper;

        public UpdatedCategoryHandler(MovieCategoryBusinessRules movieCategoryBusinessRules, IMovieCategoryRepository movieCategoryRepository, IMapper mapper)
        {
            _movieCategoryBusinessRules = movieCategoryBusinessRules;
            _movieCategoryRepository = movieCategoryRepository;
            _mapper = mapper;
        }

        public async Task<UpdatedMovieCategoryResponse> Handle(UpdatedMovieCategoryCommand request, CancellationToken cancellationToken)
        {
            await _movieCategoryBusinessRules.NoMovieCategoryFound(request.Id);

            await _movieCategoryBusinessRules.MoviesCategoryCannotBeDublicatedWhenInserted(request.MovieID, request.CategoryID);

            Domain.Entities.MoviesCategory? moviesCategory = await _movieCategoryRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            moviesCategory = _mapper.Map(request, moviesCategory);

            var updatedEntity = await _movieCategoryRepository.UpdateAsync(moviesCategory);

            var result = _mapper.Map<UpdatedMovieCategoryResponse>(updatedEntity);

            return result;
        }
    }
}
