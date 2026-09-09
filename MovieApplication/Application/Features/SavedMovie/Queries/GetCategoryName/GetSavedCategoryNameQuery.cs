using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Queries.GetCategoryName;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SavedMovie.Queries.GetCategoryName
{
    public class GetSavedCategoryNameQuery : IRequest<List<GetSavedCategoryNameDto>>, ILoggableRequest, ICachableRequest
    {

        public PageRequest? pageRequest { get; set; }
        public string? CategoryName { get; set; }
        public int UserId { get; set; }
        public string? CacheKey => $"GetSavedCategoryNameQuery UserID:{UserId} CategoryName:{CategoryName ?? "All"} Index:{pageRequest?.PageIndex} Size:{pageRequest?.PageSize}";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "SavedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetFavoriteCategoryNameHandler : IRequestHandler<GetSavedCategoryNameQuery, List<GetSavedCategoryNameDto>>
    {
        private readonly ISavedMovieRepository _savedMovieRepository;
        private readonly IMovieCategoryRepository _movieCategoryRepository;

        public GetFavoriteCategoryNameHandler(ISavedMovieRepository savedMovieRepository, IMovieCategoryRepository movieCategoryRepository)
        {
            _savedMovieRepository = savedMovieRepository;
            _movieCategoryRepository = movieCategoryRepository;
        }

        public async Task<List<GetSavedCategoryNameDto>> Handle(GetSavedCategoryNameQuery request, CancellationToken cancellationToken)
        {
            int pageIndex = request.pageRequest?.PageIndex ?? 0;
            int pageSize = request.pageRequest?.PageSize ?? 10;

            var entities = await _savedMovieRepository.GetListAsync(predicate: n => n.UserID == request.UserId);
            var savedMovieId = entities.Items.Select(n => n.MovieID).ToList();

            var entities2 = await _movieCategoryRepository.GetListAsync(
                predicate: n => savedMovieId.Contains(n.MovieID) && (string.IsNullOrEmpty(request.CategoryName) || n.Category.CategoryName == request.CategoryName),
                include: cd => cd.Include(n => n.Category).Include(n => n.Movie),
                index: pageIndex,
                size: pageSize);

            var result = new List<GetSavedCategoryNameDto>();

            foreach (var item in entities2.Items)
            {
                var movie = new GetSavedCategoryNameDto
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
