using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Features.NotificationSettings.Queries.GetList
{
    public class GetListNotificationSettingsQuery : IRequest<GetListResponse<GetListNotificationSettingsDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest? pageRequest { get; set; }

        public string? CacheKey => $"GetListNotificationSettingsQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "NotificationSettings";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListNotificationSettingsQueryHandler : IRequestHandler<GetListNotificationSettingsQuery, GetListResponse<GetListNotificationSettingsDto>>
    {
        private readonly IMapper _mapper;
        private readonly INotificationSettingsRepository _notificationSettingsRepository;

        public GetListNotificationSettingsQueryHandler(IMapper mapper, INotificationSettingsRepository notificationSettingsRepository)
        {
            _mapper = mapper;
            _notificationSettingsRepository = notificationSettingsRepository;
        }

        public async Task<GetListResponse<GetListNotificationSettingsDto>> Handle(GetListNotificationSettingsQuery request, CancellationToken cancellationToken)
        {
            var list = await _notificationSettingsRepository.GetListAsync(
                index: request.pageRequest?.PageIndex ?? 0,
                size: request.pageRequest?.PageSize ?? 10,
                predicate: null
            );

            var result = new GetListResponse<GetListNotificationSettingsDto>();

            foreach (var item in list.Items)
            {
                var entity = new GetListNotificationSettingsDto();
                entity.EntityID = item.EntityID;
                entity.UserId = item.UserId;
                entity.IsNotificationEnabled = item.IsNotificationEnabled;
                entity.CreatedAt = item.CreatedAt;
                entity.ChannelId = item.ChannelId;

                result.Items.Add(entity);
            }

            return result;
        }
    }
}
