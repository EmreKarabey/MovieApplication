using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.SavedMovie.Queries.IsSaved
{
    public class GetMovieIsSavedQuery : IRequest<bool>, ICachableRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }
        public int UserID { get; set; }

        public string CacheKey => $"GetMovieIsSavedQuery Movie_Id:{MovieID}, User_Id:{UserID}";

        public string? CacheGroupKey => "SavedMovies";

        public bool ByPassCache => true;

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetMovieIsSavedHandler : IRequestHandler<GetMovieIsSavedQuery, bool>
    {
        private readonly ISavedMovieRepository _savedMovieRepository;

        public GetMovieIsSavedHandler(ISavedMovieRepository savedMovieRepository)
        {
            _savedMovieRepository = savedMovieRepository;
        }

        public async Task<bool> Handle(GetMovieIsSavedQuery request, CancellationToken cancellationToken)
        {
            bool result = await _savedMovieRepository.AnyAsync(predicate: n => n.MovieID == request.MovieID && n.UserID == request.UserID, enableTracking: false, withDeleted: false, cancellationToken: cancellationToken);
            return result;
        }
    }
}
