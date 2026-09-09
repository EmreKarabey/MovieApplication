using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Command.Delete;
using Application.Features.Comment.Rules;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.LikedMovie.Command.Delete
{
    public class DeletedLikedMovieCommand : IRequest<DeletedLikedMovieResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"DeletedLikedMovieCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "LikedMovies";
    }

    public class DeletedLikedMovieHandler : IRequestHandler<DeletedLikedMovieCommand, DeletedLikedMovieResponse>
    {
        private readonly ILikedMovieRepository _likedMovieRepository;
        private readonly IMapper _mapper;
        private readonly LikedMovieBusinessRules _likedMovieBusinessRules;

        public DeletedLikedMovieHandler(ILikedMovieRepository likedMovieRepository, IMapper mapper, LikedMovieBusinessRules likedMovieBusinessRules)
        {
            _likedMovieRepository = likedMovieRepository;
            _mapper = mapper;
            _likedMovieBusinessRules = likedMovieBusinessRules;
        }

        public async Task<DeletedLikedMovieResponse> Handle(DeletedLikedMovieCommand request, CancellationToken cancellationToken)
        {
            await _likedMovieBusinessRules.NoLikedMovieFound(request.Id);

            Domain.Entities.LikedMovie likedMovie = await _likedMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteLikedMovie = await _likedMovieRepository.DeleteAsync(likedMovie, permanent: false);

            var result = _mapper.Map<DeletedLikedMovieResponse>(deleteLikedMovie);

            return result;
        }
    }

}
