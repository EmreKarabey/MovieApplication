using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Command.Create
{
    public class CreatedAnnouncementResponse
    {
        public Guid EntityID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subtitle1 { get; set; }
        public string Subtitle2 { get; set; }
    }
}
