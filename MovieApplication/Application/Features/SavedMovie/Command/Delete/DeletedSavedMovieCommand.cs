using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Rules;
using Application.Features.SavedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.SavedMovie.Command.Delete
{
    public class DeletedSavedMovieCommand : IRequest<DeletedSavedMovieResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"DeletedSavedMovieCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SavedMovies";
    }

    public class DeletedSavedMovieHandler : IRequestHandler<DeletedSavedMovieCommand, DeletedSavedMovieResponse>
    {
        private readonly ISavedMovieRepository _savedMovieRepository;
        private readonly IMapper _mapper;
        private readonly SavedMovieBusinessRules _savedMovieBusinessRules;

        public DeletedSavedMovieHandler(ISavedMovieRepository savedMovieRepository, IMapper mapper, SavedMovieBusinessRules savedMovieBusinessRules)
        {
            _savedMovieRepository = savedMovieRepository;
            _mapper = mapper;
            _savedMovieBusinessRules = savedMovieBusinessRules;
        }

        public async Task<DeletedSavedMovieResponse> Handle(DeletedSavedMovieCommand request, CancellationToken cancellationToken)
        {
            await _savedMovieBusinessRules.NoSavedMovieFound(request.Id);

            Domain.Entities.SavedMovie savedMovie = await _savedMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteSavedMovie = await _savedMovieRepository.DeleteAsync(savedMovie, permanent: false);

            var result = _mapper.Map<DeletedSavedMovieResponse>(deleteSavedMovie);

            return result;
        }
    }
}
