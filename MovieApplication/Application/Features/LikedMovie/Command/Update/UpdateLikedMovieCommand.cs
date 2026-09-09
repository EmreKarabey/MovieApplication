using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Command.Update;
using Application.Features.Comment.Rules;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.LikedMovie.Command.Update
{
    public class UpdateLikedMovieCommand : IRequest<UpdateLikedMovieResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }

        public Guid MovieID { get; set; }

        public string? CacheKey => $"UpdateLikedMovieCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "LikedMovies";
    }

    public class UpdateLikedMovieHandler : IRequestHandler<UpdateLikedMovieCommand, UpdateLikedMovieResponse>
    {
        private readonly LikedMovieBusinessRules _likedMovieBusinessRules;
        private readonly ILikedMovieRepository _likedMovieRepository;
        private readonly IMapper _mapper;

        public UpdateLikedMovieHandler(LikedMovieBusinessRules likedMovieBusinessRules, ILikedMovieRepository likedMovieRepository, IMapper mapper)
        {
            _likedMovieBusinessRules = likedMovieBusinessRules;
            _likedMovieRepository = likedMovieRepository;
            _mapper = mapper;
        }

        public async Task<UpdateLikedMovieResponse> Handle(UpdateLikedMovieCommand request, CancellationToken cancellationToken)
        {

            await _likedMovieBusinessRules.NoLikedMovieFound(request.Id);

            await _likedMovieBusinessRules.UserAuthentication(request.Id);

            Domain.Entities.LikedMovie likedMovie = await _likedMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            likedMovie = _mapper.Map(request, likedMovie);

            var updateLikedMovie = await _likedMovieRepository.UpdateAsync(likedMovie);

            var result = _mapper.Map<UpdateLikedMovieResponse>(updateLikedMovie);

            return result;
        }
    }

}
