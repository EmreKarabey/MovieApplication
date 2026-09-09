using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Movies.Rules;
using Application.Services;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Movies.Commands.Update
{
    public class UpdatedMovieCommand : IRequest<UpdatedMovieResponse>, ILoggableRequest
    {
        public Guid EntityID { get; set; }
        public string Name { get; set; }
        public IFormFile ImageFile { get; set; }
        public IFormFile VideoFile { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }
        public int PublisherId { get; set; }
    }

    public class UpdatedMovieHandler : IRequestHandler<UpdatedMovieCommand, UpdatedMovieResponse>
    {
        private readonly MoviesBusinessRules _moviesBusinessRules;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        public UpdatedMovieHandler(IMovieRepository movieRepository, IMapper mapper, MoviesBusinessRules moviesBusinessRules, ICloudinaryService cloudinaryService)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _moviesBusinessRules = moviesBusinessRules;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<UpdatedMovieResponse> Handle(UpdatedMovieCommand request, CancellationToken cancellationToken)
        {
            await _moviesBusinessRules.MovieNameCannotBeDuplicatedWhenInserted(request.Name);

            await _moviesBusinessRules.NoMovieFound(request.EntityID);

            Movie? movie = await _movieRepository.GetAsync(predicate: n => n.EntityID == request.EntityID, cancellationToken: cancellationToken);

            var uri = new Uri(movie.VideoURL);
            var segments = uri.AbsolutePath.Split('/');
            var publicId = string.Join("/", segments.Skip(Array.IndexOf(segments, "upload") + 2))
                                .Replace(".mp4", "").Replace(".mov", "");

            var imageUri = new Uri(movie.ImageURL);
            var imageSegments = imageUri.AbsolutePath.Split('/');
            var imagePublicId = string.Join("/", imageSegments.Skip(Array.IndexOf(imageSegments, "upload") + 2))
                                    .Replace(".jpg", "").Replace(".png", "").Replace(".webp", "");


            await _cloudinaryService.DeleteAsync(publicId);
            await _cloudinaryService.DeleteImageAsync(imagePublicId);

            movie = _mapper.Map(request, movie);

            var url = await _cloudinaryService.UploadAsync(request.VideoFile);

            var Imageurl = await _cloudinaryService.UploadImageAsync(request.ImageFile);

            movie.VideoURL = url;
            movie.ImageURL = Imageurl;

            await _movieRepository.UpdateAsync(movie);

            var result = _mapper.Map<UpdatedMovieResponse>(movie);

            return result;

        }
    }
}
