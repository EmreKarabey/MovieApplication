using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Activities.Constants;
using Application.Features.FavoriteMovie.Constants;
using Application.Services.Repositories;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Activities.Rules
{
    public class ActivitiesBusinessRules
    {
        private readonly IActivitiesRepository _activitiesRepository;

        public ActivitiesBusinessRules(IActivitiesRepository activitiesRepository)
        {
            _activitiesRepository = activitiesRepository;
        }

        public async Task NoActivityFound(Guid Id)
        {
            var activities = await _activitiesRepository.GetAsync(predicate: n => n.MovieID == Id);

            if (activities == null) throw new BusinessException(ActivitiesMessages.NoActivityFound);
        }
    }
}
