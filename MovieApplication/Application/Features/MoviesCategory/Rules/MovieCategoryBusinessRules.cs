using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Category.Constants;
using Application.Features.MoviesCategory.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MoviesCategory.Rules
{
    public class MovieCategoryBusinessRules : BaseBusinessRules
    {
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        public MovieCategoryBusinessRules(IMovieCategoryRepository movieCategoryRepository)
        {
            _movieCategoryRepository = movieCategoryRepository;
        }

        public async Task MoviesCategoryCannotBeDublicatedWhenInserted(Guid movieID, Guid categoryID)
        {
            Domain.Entities.MoviesCategory moviesCategory = await _movieCategoryRepository.GetAsync(predicate: n => n.MovieID == movieID && n.CategoryID == categoryID);

            if (moviesCategory != null) throw new BusinessException(MovieCategoryMessages.MovieCategoryNameExists);
        }

        public async Task NoMovieCategoryFound(Guid Id)
        {
            Domain.Entities.MoviesCategory moviesCategory = await _movieCategoryRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (moviesCategory == null) throw new BusinessException(MovieCategoryMessages.NoMovieCategoryFound);
        }

        public async Task NoCategoryNameFound(string CategoryName)
        {
            Domain.Entities.MoviesCategory moviesCategory = await _movieCategoryRepository.GetAsync(
                predicate: n => n.Category.CategoryName == CategoryName,
                include: q => q.Include(x => x.Category)
            );

            if (moviesCategory == null) throw new BusinessException(MovieCategoryMessages.NoCategoryNameFound);
        }
    }
}
