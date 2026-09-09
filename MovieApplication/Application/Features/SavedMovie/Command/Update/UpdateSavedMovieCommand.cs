using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Rules;
using Application.Features.SavedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.SavedMovie.Command.Update
{
    public class UpdateSavedMovieCommand : IRequest<UpdateSavedMovieResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }

        public Guid MovieID { get; set; }

        public string? CacheKey => $"UpdateSavedMovieCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SavedMovies";
    }

    public class UpdateSavedMovieHandler : IRequestHandler<UpdateSavedMovieCommand, UpdateSavedMovieResponse>
    {
        private readonly SavedMovieBusinessRules _savedMovieBusinessRules;
        private readonly ISavedMovieRepository _savedMovieRepository;
        private readonly IMapper _mapper;

        public UpdateSavedMovieHandler(SavedMovieBusinessRules savedMovieBusinessRules, ISavedMovieRepository savedMovieRepository, IMapper mapper)
        {
            _savedMovieBusinessRules = savedMovieBusinessRules;
            _savedMovieRepository = savedMovieRepository;
            _mapper = mapper;
        }

        public async Task<UpdateSavedMovieResponse> Handle(UpdateSavedMovieCommand request, CancellationToken cancellationToken)
        {

            await _savedMovieBusinessRules.NoSavedMovieFound(request.Id);

            await _savedMovieBusinessRules.UserAuthentication(request.Id);

            Domain.Entities.SavedMovie savedMovie = await _savedMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            savedMovie = _mapper.Map(request, savedMovie);

            var updateLikedMovie = await _savedMovieRepository.UpdateAsync(savedMovie);

            var result = _mapper.Map<UpdateSavedMovieResponse>(updateLikedMovie);

            return result;
        }
    }
}
