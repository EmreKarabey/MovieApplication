using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.SavedMovie.Command.DeleteIdMovie
{
    public class DeleteIdMovieCommand : IRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }
        public int UserID { get; set; }

        public string? CacheKey => $"DeleteIdMovieCommand Movie_Id:{MovieID}, User_Id:{UserID}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SavedMovies";
    }

    public class DeleteIdMovieHandler : IRequestHandler<DeleteIdMovieCommand>
    {
        private readonly ISavedMovieRepository _savedMovieRepository;

        public DeleteIdMovieHandler(ISavedMovieRepository savedMovieRepository)
        {
            _savedMovieRepository = savedMovieRepository;
        }

        public async Task Handle(DeleteIdMovieCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.SavedMovie? entity = await _savedMovieRepository.GetAsync(predicate: n => n.MovieID == request.MovieID && n.UserID == request.UserID);

            await _savedMovieRepository.DeleteAsync(entity, permanent: true);
        }
    }
}
