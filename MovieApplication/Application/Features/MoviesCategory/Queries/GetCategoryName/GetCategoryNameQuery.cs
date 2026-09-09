using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using AutoMapper;
using Application.Features.MoviesCategory.Rules;
using CoreApplication.Request;
namespace Application.Features.MoviesCategory.Queries.GetCategoryName
{
    public class GetCategoryNameQuery : IRequest<List<GetCategoryNameDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest? pageRequest { get; set; }
        public string? CategoryName { get; set; }

        public string? CacheKey => $"GetCategoryNameQuery CategoryName:{CategoryName ?? "All"} Index:{pageRequest?.PageIndex} Size:{pageRequest?.PageSize}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "MoviesCategories";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetCategoryNameHandler : IRequestHandler<GetCategoryNameQuery, List<GetCategoryNameDto>>
    {
        private readonly IMovieCategoryRepository _movieCategoryRepository;
        private readonly IMapper _mapper;
        private readonly MovieCategoryBusinessRules _movieCategoryBusinessRules;

        public GetCategoryNameHandler(IMovieCategoryRepository movieCategoryRepository, IMapper mapper, MovieCategoryBusinessRules movieCategoryBusinessRules)
        {
            _movieCategoryRepository = movieCategoryRepository;
            _mapper = mapper;
            _movieCategoryBusinessRules = movieCategoryBusinessRules;
        }

        public async Task<List<GetCategoryNameDto>> Handle(GetCategoryNameQuery request, CancellationToken cancellationToken)
        {
            int pageIndex = request.pageRequest?.PageIndex ?? 0;
            int pageSize = request.pageRequest?.PageSize ?? 10;

            var entities = await _movieCategoryRepository.GetListAsync(predicate: null, include: q => q.Include(n => n.Category).Include(n => n.Movie), index: pageIndex, size: pageSize);
            if (!string.IsNullOrEmpty(request.CategoryName))
            {

                entities = await _movieCategoryRepository.GetListAsync(predicate: n => n.Category.CategoryName == request.CategoryName, include: cd => cd.Include(n => n.Category).Include(n => n.Movie), index: pageIndex, size: pageSize);

                var result = new List<GetCategoryNameDto>();

                foreach (var item in entities.Items)
                {
                    var movie = new GetCategoryNameDto
                    {
                        EntityID = item.Movie.EntityID,
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

            var result2 = new List<GetCategoryNameDto>();

            foreach (var item in entities.Items)
            {
                var movie = new GetCategoryNameDto
                {
                    EntityID = item.MovieID,
                    Description = item.Movie.Description,
                    ImageURL = item.Movie.ImageURL,
                    Name = item.Movie.Name,
                    ProducerName = item.Movie.ProducerName,
                    VideoURL = item.Movie.VideoURL,
                    ReleaseDate = item.Movie.ReleaseDate
                };

                result2.Add(movie);
            }
            return result2;
        }
    }
}
