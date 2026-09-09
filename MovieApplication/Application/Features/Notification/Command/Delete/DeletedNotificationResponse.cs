using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Notification.Command.Delete
{
    public class DeletedNotificationResponse
    {
        public Guid EntityID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
    }
}
