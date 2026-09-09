using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Rules;
using Application.Features.Movies.Rules;
using Application.Services;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Authorization;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreApplication.Request;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Application.Features.Movies.Commands.Create
{
    public class CreatedMovieCommand : IRequest<CreatedMovieResponse>, ICacheRemoveRequest, ILoggableRequest, ITransactionalRequest, ISecuredRequest
    {
        public string Name { get; set; }
        public IFormFile VideoFile { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }

        public string? CacheKey => $"CreatedMovieCommand Name:{Name}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Movies";

        public string[] Roles => new[] { "Publisher" };
    }
    public class CreatedMovieHandler : IRequestHandler<CreatedMovieCommand, CreatedMovieResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly MoviesBusinessRules _moviesBusinessRules;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly LoginBusinessRules _loginBusinessRules;

        public CreatedMovieHandler(IMovieRepository movieRepository, IMapper mapper, MoviesBusinessRules moviesBusinessRules, ICloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _moviesBusinessRules = moviesBusinessRules;
            _cloudinaryService = cloudinaryService;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<CreatedMovieResponse> Handle(CreatedMovieCommand request, CancellationToken cancellationToken)
        {
            var UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var currentUserId = int.Parse(UserId);


            await _loginBusinessRules.NoUserFound(currentUserId);
            await _moviesBusinessRules.MovieNameCannotBeDuplicatedWhenInserted(request.Name);

            var VideoUploadTask = _cloudinaryService.UploadAsync(request.VideoFile);
            var ImageUploadTask = _cloudinaryService.UploadImageAsync(request.ImageFile);

            await Task.WhenAll(VideoUploadTask, ImageUploadTask);

            var videoUrl = VideoUploadTask.Result;
            var imageUrl = ImageUploadTask.Result;

            Movie? movie = _mapper.Map<Movie>(request);

            movie.VideoURL = videoUrl;
            movie.ImageURL = imageUrl;
            movie.PublisherId = currentUserId;

            var entity = await _movieRepository.AddAsync(movie);




            var result = _mapper.Map<CreatedMovieResponse>(entity);

            return result;
        }
    }
}
