using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Announcement.Queries.GetLİst;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.Announcement.Queries.GetList
{
    public class GetListAnnouncementQuery : IRequest<List<GetListAnnouncementDto>>, ICachableRequest, ILoggableRequest
    {

        public string? CacheKey => $"GetListAnnouncementQuery";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Announcements";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListAnnouncementHandler : IRequestHandler<GetListAnnouncementQuery, List<GetListAnnouncementDto>>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;

        public GetListAnnouncementHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }

        public async Task<List<GetListAnnouncementDto>> Handle(GetListAnnouncementQuery request, CancellationToken cancellationToken)
        {
            var list = await _announcementRepository.Take2Async(predicate: null);

            var result = _mapper.Map<List<GetListAnnouncementDto>>(list);

            return result;
        }
    }
}
