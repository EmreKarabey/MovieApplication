using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.NotificationSettings.Command.ChangeSettings
{
    public class ChangeNotificationSettingsResponse
    {
        public Guid EntityID { get; set; }
        public int UserId { get; set; }
        public int ChannelId { get; set; }
        public bool IsNotificationEnabled { get; set; }
    }
}
