using System;

namespace Application.Features.Notification.Queries.GetById
{
    public class GetByIdNotificationDto
    {
        public Guid EntityID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public bool IsRead { get; set; }
    }
}
