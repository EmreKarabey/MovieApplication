using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Category.Command.Delete;
using Application.Features.Category.Rules;
using Application.Features.MoviesCategory.Constants;
using Application.Features.MoviesCategory.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.MoviesCategory.Commands.Delete
{
    public class DeletedMovieCategoryCommand : IRequest<DeletedMovieCategoryResponse>
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"DeletedMovieCategoryCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "MoviesCategories";
    }

    public class DeletedMovieCategoryHandler : IRequestHandler<DeletedMovieCategoryCommand, DeletedMovieCategoryResponse>
    {
        private readonly MovieCategoryBusinessRules _movieCategoryBusinessRules;
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        private readonly IMapper _mapper;

        public DeletedMovieCategoryHandler(MovieCategoryBusinessRules movieCategoryBusinessRules, IMovieCategoryRepository movieCategoryRepository, IMapper mapper)
        {
            _movieCategoryBusinessRules = movieCategoryBusinessRules;
            _movieCategoryRepository = movieCategoryRepository;
            _mapper = mapper;
        }

        public async Task<DeletedMovieCategoryResponse> Handle(DeletedMovieCategoryCommand request, CancellationToken cancellationToken)
        {
            await _movieCategoryBusinessRules.NoMovieCategoryFound(request.Id);

            Domain.Entities.MoviesCategory moviesCategory = await _movieCategoryRepository.GetAsync(predicate: n => n.EntityID == request.Id, cancellationToken: cancellationToken);

            var deleteEntity = await _movieCategoryRepository.DeleteAsync(moviesCategory);

            var result = _mapper.Map<DeletedMovieCategoryResponse>(deleteEntity);

            return result;
        }
    }
}
