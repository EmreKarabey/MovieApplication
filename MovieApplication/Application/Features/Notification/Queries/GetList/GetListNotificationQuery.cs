using System.Threading;
using System.Threading.Tasks;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using MediatR;

namespace Application.Features.Notification.Queries.GetList
{
    public class GetListNotificationQuery : IRequest<GetListResponse<GetListNotificationDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListNotificationQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";
        public bool ByPassCache { get; }
        public string? CacheGroupKey => "Notification";
        public System.TimeSpan? SlidingExpiration { get; }
    }

    public class GetListNotificationQueryHandler : IRequestHandler<GetListNotificationQuery, GetListResponse<GetListNotificationDto>>
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;

        public GetListNotificationQueryHandler(IMapper mapper, INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task<GetListResponse<GetListNotificationDto>> Handle(GetListNotificationQuery request, CancellationToken cancellationToken)
        {
            var list = await _notificationRepository.GetListAsync(
                index: request.pageRequest.PageIndex,
                size: request.pageRequest.PageSize,
                predicate: null
            );

            var result = new GetListResponse<GetListNotificationDto>();
            foreach (var item in list.Items)
            {
                var entity = new GetListNotificationDto();
                entity.EntityID = item.EntityID;
                entity.UserID = item.UserID;
                entity.Message = item.Message;
                entity.Url = item.Url;
                entity.IsRead = item.IsRead;

                result.Items.Add(entity);
            }
            return result;
        }
    }
}
