using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.NotificationSettings.Queries.GetList
{
    public class GetListNotificationSettingsDto
    {
        public Guid EntityID { get; set; }
        public int UserId { get; set; }
        public int ChannelId { get; set; }
        public bool IsNotificationEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
