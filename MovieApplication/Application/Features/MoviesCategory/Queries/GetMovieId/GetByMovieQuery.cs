using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Movies.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MoviesCategory.Queries.GetMovieId
{
    public class GetByMovieQuery : IRequest<GetByMovieDto>, ICachableRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }

        public string? CacheKey => $"GetByMovieQuery MovieId:{MovieID}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "MoviesCategories";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByMovieHandler : IRequestHandler<GetByMovieQuery, GetByMovieDto>
    {
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        private readonly MoviesBusinessRules _moviesBusinessRules;

        public GetByMovieHandler(IMovieCategoryRepository movieCategoryRepository, MoviesBusinessRules moviesBusinessRules)
        {
            _movieCategoryRepository = movieCategoryRepository;
            _moviesBusinessRules = moviesBusinessRules;
        }

        public async Task<GetByMovieDto> Handle(GetByMovieQuery request, CancellationToken cancellationToken)
        {
            await _moviesBusinessRules.NoMovieFound(request.MovieID);

            var entites = await _movieCategoryRepository.GetListAsync(predicate: n => n.MovieID == request.MovieID, include: n => n.Include(z => z.Movie).Include(z => z.Category));

            var result = new GetByMovieDto();

            result.CategoryName = new List<string>();

            foreach (var item in entites.Items)
            {
                result.CategoryName.Add(item.Category.CategoryName);
                result.MovieName = item.Movie.Name;
                result.ReleaseDate = item.Movie.ReleaseDate;
                result.Description = item.Movie.Description;
                result.ImageURL = item.Movie.ImageURL;
                result.ProducerName = item.Movie.ProducerName;
            }
            return result;
        }
    }
}
