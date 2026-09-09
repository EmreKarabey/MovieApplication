using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Constants;
using Application.Features.Movies.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;

namespace Application.Features.Movies.Rules
{
    public class MoviesBusinessRules : BaseBusinessRules
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesBusinessRules(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task MovieNameCannotBeDuplicatedWhenInserted(string name)
        {
            var entity = await _movieRepository.GetAsync(predicate: n => n.Name == name);

            if (entity != null) throw new BusinessException(MoviesMessages.MovieNameExists);

        }

        public async Task NoMovieFound(Guid Id)
        {
            Movie movie = await _movieRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (movie == null) throw new BusinessException(MoviesMessages.NoMovieFound);
        }
    }
}
