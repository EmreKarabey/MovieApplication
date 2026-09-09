using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Notification.Queries.GetMyNotificationList
{
    public class GetMyNotificationListDto
    {
        public Guid EntityID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public bool IsRead { get; set; }
    }
}
