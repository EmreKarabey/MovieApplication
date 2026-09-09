using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Activities.Queries.GetList;
using Application.Features.Activities.Queries.MyActivity;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Activities.Queries.MyActivities
{
    public class GetMyActivitiesQuery : IRequest<GetListResponse<GetMyActivitiesDto>>, ICachableRequest, ILoggableRequest
    {
        public PageRequest PageRequest { get; set; }

        public int UserID { get; set; }

        public string CacheKey => $"GetMyActivitiesQuery({UserID},{PageRequest.PageIndex},{PageRequest.PageSize})";
        public bool ByPassCache { get; }
        public string? CacheGroupKey => "Activities";
        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListActivitiesQueryHandler : IRequestHandler<GetMyActivitiesQuery, GetListResponse<GetMyActivitiesDto>>
    {
        private readonly IActivitiesRepository _activitiesRepository;

        public GetListActivitiesQueryHandler(IActivitiesRepository activitiesRepository)
        {
            _activitiesRepository = activitiesRepository;
        }

        public async Task<GetListResponse<GetMyActivitiesDto>> Handle(GetMyActivitiesQuery request, CancellationToken cancellationToken)
        {
            Paginate<Domain.Entities.Activities> activities = await _activitiesRepository.GetListAsync(
                predicate: n => n.UserID == request.UserID,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                orderBy: n => n.OrderByDescending(p => p.CreatedAt),
                include: n => n.Include(p => p.Movie),
                cancellationToken: cancellationToken
            );

            GetListResponse<GetMyActivitiesDto> getListResponse = new();

            foreach (var item in activities.Items)
            {
                GetMyActivitiesDto entity = new GetMyActivitiesDto();

                entity.Id = item.EntityID;
                entity.MovieName = item.Movie.Name;
                entity.ImageURL = item.Movie.ImageURL;
                entity.CreatedAt = item.CreatedAt;
                entity.UserID = item.UserID;
                entity.MovieID = item.MovieID;
                entity.ActivitiesCategory = (int)item.ActivitiesCategory;

                getListResponse.Items.Add(entity);

            }

            getListResponse.PageSize = activities.Size;
            getListResponse.PageIndex = activities.Index;

            return getListResponse;
        }
    }
}
