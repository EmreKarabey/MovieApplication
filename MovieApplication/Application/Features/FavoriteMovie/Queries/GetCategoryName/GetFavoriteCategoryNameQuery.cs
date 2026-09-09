using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.MoviesCategory.Queries.GetCategoryName;
using Application.Features.MoviesCategory.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.FavoriteMovie.Queries.GetCategoryName
{
    public class GetFavoriteCategoryNameQuery : IRequest<List<GetFavoriteCategoryNameDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest? pageRequest { get; set; }
        public string? CategoryName { get; set; }
        public int UserId { get; set; }
        public string? CacheKey => $"GetFavoriteCategoryNameQuery UserID:{UserId} CategoryName:{CategoryName ?? "All"} Index:{pageRequest?.PageIndex} Size:{pageRequest?.PageSize}";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "FavoriteMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetFavoriteCategoryNameHandler : IRequestHandler<GetFavoriteCategoryNameQuery, List<GetFavoriteCategoryNameDto>>
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        public GetFavoriteCategoryNameHandler(IFavoriteMovieRepository favoriteMovieRepository, IMovieCategoryRepository movieCategoryRepository)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
            _movieCategoryRepository = movieCategoryRepository;
        }

        public async Task<List<GetFavoriteCategoryNameDto>> Handle(GetFavoriteCategoryNameQuery request, CancellationToken cancellationToken)
        {
            int pageIndex = request.pageRequest?.PageIndex ?? 0;
            int pageSize = request.pageRequest?.PageSize ?? 10;

            var entities = await _favoriteMovieRepository.GetListAsync(predicate: n => n.UserID == request.UserId);
            var favoriteMovieId = entities.Items.Select(n => n.MovieID).ToList();

            var entities2 = await _movieCategoryRepository.GetListAsync(
                predicate: n => favoriteMovieId.Contains(n.MovieID) && (string.IsNullOrEmpty(request.CategoryName) || n.Category.CategoryName == request.CategoryName),
                include: cd => cd.Include(n => n.Category).Include(n => n.Movie),
                index: pageIndex,
                size: pageSize);

            var result = new List<GetFavoriteCategoryNameDto>();

            foreach (var item in entities2.Items)
            {
                var movie = new GetFavoriteCategoryNameDto
                {
                    EntityID = item.MovieID,
                    Description = item.Movie.Description,
                    ImageURL = item.Movie.ImageURL,
                    Name = item.Movie.Name,
                    ProducerName = item.Movie.ProducerName,
                    VideoURL = item.Movie.VideoURL,
                    ReleaseDate = item.Movie.ReleaseDate
                };

                result.Add(movie);
            }
            return result;


        }
    }
}