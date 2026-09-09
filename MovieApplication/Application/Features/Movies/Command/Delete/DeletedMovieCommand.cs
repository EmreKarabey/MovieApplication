using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Application.Features.Movies.Rules;
using Application.Services;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.Movies.Commands.Delete
{
    public class DeletedMovieCommand : IRequest<DeletedMovieResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid EntityID { get; set; }
        public bool Permanent { get; set; }

        public string? CacheKey => $"DeletedMovieCommand Id:{EntityID}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Movies";
    }
    public class DeletedMovieHandler : IRequestHandler<DeletedMovieCommand, DeletedMovieResponse>
    {
        private readonly MoviesBusinessRules _moviesBusinessRules;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public DeletedMovieHandler(IMovieRepository movieRepository, IMapper mapper, MoviesBusinessRules moviesBusinessRules, ICloudinaryService cloudinaryService)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _moviesBusinessRules = moviesBusinessRules;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<DeletedMovieResponse> Handle(DeletedMovieCommand request, CancellationToken cancellationToken)
        {
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

            if (request.Permanent)
            {
                await _cloudinaryService.DeleteAsync(publicId);

                await _cloudinaryService.DeleteImageAsync(imagePublicId);
            }




            await _movieRepository.DeleteAsync(movie, permanent: request.Permanent);

            var result = _mapper.Map<DeletedMovieResponse>(movie);
            return result;
        }
    }

}
