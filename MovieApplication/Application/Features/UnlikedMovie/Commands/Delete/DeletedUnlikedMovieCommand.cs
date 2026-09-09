using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Command.Delete;
using Application.Features.Comment.Rules;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.LikedMovie.Rules;
using Application.Features.UnlikedMovie.Command.Delete;
using Application.Features.UnlikedMovie.Constants;
using Application.Features.UnlikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.LikedMovie.Command.Delete
{
    public class DeletedUnlikedMovieCommand : IRequest<DeletedUnlikedMovieResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"DeletedUnlikedMovieCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "UnlikedMovies";
    }

    public class DeletedUnlikedMovieHandler : IRequestHandler<DeletedUnlikedMovieCommand, DeletedUnlikedMovieResponse>
    {
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;
        private readonly IMapper _mapper;
        private readonly UnlikedBusinessRules _unlikedBusinessRules;

        public DeletedUnlikedMovieHandler(IUnlikedMovieRepository unlikedMovieRepository, IMapper mapper, UnlikedBusinessRules unlikedBusinessRules)
        {
            _unlikedMovieRepository = unlikedMovieRepository;
            _mapper = mapper;
            _unlikedBusinessRules = unlikedBusinessRules;
        }

        public async Task<DeletedUnlikedMovieResponse> Handle(DeletedUnlikedMovieCommand request, CancellationToken cancellationToken)
        {
            await _unlikedBusinessRules.NoUnlikedMovieFound(request.Id);

            Domain.Entities.UnlikedMovie unlikedMovie = await _unlikedMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteLikedMovie = await _unlikedMovieRepository.DeleteAsync(unlikedMovie, permanent: false);

            var result = _mapper.Map<DeletedUnlikedMovieResponse>(deleteLikedMovie);

            return result;
        }
    }

}
