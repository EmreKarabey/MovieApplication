using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Notification.Queries.GetMyNotificationList
{
    public class GetMyNotificationListQuery : IRequest<List<GetMyNotificationListDto>>, ILoggableRequest
    {
        public int UserId { get; set; }
    }
    public class GetMyNotificationListHandler : IRequestHandler<GetMyNotificationListQuery, List<GetMyNotificationListDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public GetMyNotificationListHandler(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<List<GetMyNotificationListDto>> Handle(GetMyNotificationListQuery request, CancellationToken cancellationToken)
        {
            var list = await _notificationRepository.MyNotificationList(request.UserId);
            return _mapper.Map<List<GetMyNotificationListDto>>(list);
        }
    }
}
