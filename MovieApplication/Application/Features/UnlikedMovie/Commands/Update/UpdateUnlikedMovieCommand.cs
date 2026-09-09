using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Rules;
using Application.Features.UnlikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.UnlikedMovie.Commands.Update
{
    public class UpdateUnlikedMovieCommand : IRequest<UpdateUnlikedMovieResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }

        public Guid MovieID { get; set; }

        public string? CacheKey => $"UpdateUnlikedMovieCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "UnlikedMovies";
    }

    public class UpdateUnlikedMovieHandler : IRequestHandler<UpdateUnlikedMovieCommand, UpdateUnlikedMovieResponse>
    {
        private readonly UnlikedBusinessRules _unlikedBusinessRules;
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;
        private readonly IMapper _mapper;

        public UpdateUnlikedMovieHandler(UnlikedBusinessRules unlikedBusinessRules, IUnlikedMovieRepository unlikedMovieRepository, IMapper mapper)
        {
            _unlikedBusinessRules = unlikedBusinessRules;
            _unlikedMovieRepository = unlikedMovieRepository;
            _mapper = mapper;
        }

        public async Task<UpdateUnlikedMovieResponse> Handle(UpdateUnlikedMovieCommand request, CancellationToken cancellationToken)
        {

            await _unlikedBusinessRules.NoUnlikedMovieFound(request.Id);

            await _unlikedBusinessRules.UserAuthentication(request.Id);

            Domain.Entities.UnlikedMovie unlikedMovie = await _unlikedMovieRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            unlikedMovie = _mapper.Map(request, unlikedMovie);

            var updateUnlikedMovie = await _unlikedMovieRepository.UpdateAsync(unlikedMovie);

            var result = _mapper.Map<UpdateUnlikedMovieResponse>(updateUnlikedMovie);

            return result;
        }
    }
}
