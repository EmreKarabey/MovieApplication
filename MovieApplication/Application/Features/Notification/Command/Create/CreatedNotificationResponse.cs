using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreSecurity.Entities;

namespace Application.Features.Notification.Command.Create
{
    public class CreatedNotificationResponse
    {
        public int UserID { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public bool IsRead { get; set; }
    }
}
