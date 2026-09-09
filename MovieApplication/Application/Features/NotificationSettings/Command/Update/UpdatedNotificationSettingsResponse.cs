using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.NotificationSettings.Command.Update
{
    public class UpdatedNotificationSettingsResponse
    {
        public Guid EntityID { get; set; }
        public int UserId { get; set; }
        public bool IsNotificationEnabled { get; set; }
    }
}
